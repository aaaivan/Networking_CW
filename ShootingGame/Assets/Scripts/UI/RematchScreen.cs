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

	public override void OnEnable()
	{
		base.OnEnable();
		RefreshPlayerLists();
		startButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
	}

	private void Awake()
	{
		foreach (var player in PhotonNetwork.PlayerList)
		{
			readyPlayerEntries.Add(Instantiate(playerEntryPrefab.gameObject, readySection));
			onlinePlayerEntries.Add(Instantiate(playerEntryPrefab.gameObject, onlineSection));
		}
	}

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
				if (player.CustomProperties.ContainsKey("readyToRematch"))
				{
					ready = (bool)player.CustomProperties["readyToRematch"];
					if (ready)
					{
						readyGO.SetActive(true);
						readyGO.GetComponent<TMP_Text>().text = player.NickName;
						++readyPlayers;
					}
				}
				if (!ready)
				{
					onlineGO.SetActive(true);
					onlineGO.GetComponent<TMP_Text>().text = player.NickName;
				}
			}

		}
		startButton.interactable = readyPlayers > 1;
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		if(changedProps.ContainsKey("readyToRematch"))
		{
			RefreshPlayerLists();
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		RefreshPlayerLists();
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		base.OnMasterClientSwitched(newMasterClient);
		startButton.gameObject.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
	}
}
