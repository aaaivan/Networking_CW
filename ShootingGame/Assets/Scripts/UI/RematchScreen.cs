using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RematchScreen : MonoBehaviourPunCallbacks
{
	[SerializeField]
	Button startButton;
	[SerializeField]
	RectTransform onlineSection;
	[SerializeField]
	RectTransform readySection;
	[SerializeField]
	TMP_Text playerEntryPrefab;

	List<GameObject> onlinePlayerEntries = new List<GameObject>();
	List<GameObject> readyPlayerEntries = new List<GameObject>();

	public const string readyToRematchKey = "readyToRematch";

	public override void OnEnable()
	{
		base.OnEnable();
		RefreshPlayerLists();
		startButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient); // only the master client can start a new game
	}

	private void Awake()
	{
		foreach (var player in PhotonNetwork.PlayerList)
		{
			readyPlayerEntries.Add(Instantiate(playerEntryPrefab.gameObject, readySection));
			onlinePlayerEntries.Add(Instantiate(playerEntryPrefab.gameObject, onlineSection));
		}
	}

	/// <summary>
	/// refresh the list of players that are online and
	/// th elist of players that are ready to rematch
	/// </summary>
	private void RefreshPlayerLists()
	{
		int readyPlayers = 0;
		for (int i = 0; i < onlinePlayerEntries.Count; ++i)
		{
			GameObject onlineGO = onlinePlayerEntries[i];
			onlineGO.SetActive(false);
			GameObject readyGO = readyPlayerEntries[i];
			readyGO.SetActive(false);
			if (i < PhotonNetwork.PlayerList.Count())
			{
				Photon.Realtime.Player player = PhotonNetwork.PlayerList[i];
				bool ready = false;

				// check whether the player is ready to rematch
				if (player.CustomProperties.ContainsKey(readyToRematchKey))
				{
					ready = (bool)player.CustomProperties[readyToRematchKey];
					if (ready)
					{
						// player is ready to rematch, add their nickname to the list of ready players
						readyGO.SetActive(true);
						readyGO.GetComponent<TMP_Text>().text = player.NickName;
						++readyPlayers;
					}
				}
				if (!ready)
				{
					// the player is online but not ready to rematch, add their name to the list of online players
					onlineGO.SetActive(true);
					onlineGO.GetComponent<TMP_Text>().text = player.NickName;
				}
			}

		}
		startButton.interactable = readyPlayers > 1; // restarting requires at least two players
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		if(changedProps.ContainsKey(readyToRematchKey)) // the ready to rematch status has changed
		{
			RefreshPlayerLists();
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		RefreshPlayerLists();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		// activate the start game button for the new master client
		startButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
	}
}
