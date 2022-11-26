using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayersInRoomList : MonoBehaviour
{
	[SerializeField]
	GameObject listEntryPrefab;
	bool playerListDirty = true; // if true, we need to update the room list

	private void OnEnable()
	{
		playerListDirty = true;
		NetworkManager.OnPlayersChanged += DirtyPlayerList;
		StartCoroutine(UpdatePlayerList());
	}

	private void OnDisable()
	{
		StopCoroutine(UpdatePlayerList());
		NetworkManager.OnPlayersChanged -= DirtyPlayerList;
		ClearPlayerEntries();
	}

	IEnumerator UpdatePlayerList()
	{
		while (true)
		{
			if(playerListDirty) // list needs to be updated
			{
				playerListDirty = false;
				ClearPlayerEntries();

				Player[] players;
				NetworkManager.Instance.PlayersGet(out players);

				// re-build the players list
				foreach(var player in players)
				{
					GameObject entry = Instantiate(listEntryPrefab, transform);
					entry.GetComponent<TMP_Text>().text = player.NickName;
				}
			}
			yield return new WaitForSeconds(1); // check if the list needs to be refreshed every 1 second
		}
	}

	private void ClearPlayerEntries()
	{
		for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
		{
			Destroy(gameObject.transform.GetChild(i).gameObject);
		}
	}

	public void DirtyPlayerList()
	{
		playerListDirty = true;
	}

}
