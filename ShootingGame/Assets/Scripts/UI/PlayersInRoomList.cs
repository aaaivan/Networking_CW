using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class PlayersInRoomList : MonoBehaviour
{
	[SerializeField]
	GameObject listEntryPrefab;
	bool playerListDirty = true;

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
			if(playerListDirty)
			{
				Debug.Log("Updating player list");
				playerListDirty = false;
				ClearPlayerEntries();

				Player[] players;
				NetworkManager.Instance.PlayersGet(out players);

				foreach(var player in players)
				{
					GameObject entry = Instantiate(listEntryPrefab, transform);
					entry.GetComponent<TMP_Text>().text = player.NickName;
				}
			}

			yield return new WaitForSeconds(1);
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
