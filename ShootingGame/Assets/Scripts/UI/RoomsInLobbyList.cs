using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomsInLobbyList : MonoBehaviour
{
	[SerializeField]
	GameObject listEntryPrefab;
	bool roomListDirty = true;

	private void OnEnable()
	{
		roomListDirty = true;
		NetworkManager.OnRoomsChanged += DirtyRoomList;
		StartCoroutine(UpdateRoomList());
	}

	private void OnDisable()
	{
		StopCoroutine(UpdateRoomList());
		NetworkManager.OnRoomsChanged -= DirtyRoomList;
		ClearRoomEntries();
	}

	IEnumerator UpdateRoomList()
	{
		while (true)
		{
			if (roomListDirty) // rooms list needs to be updated
			{
				Debug.Log("Updating room list");
				roomListDirty = false;
				ClearRoomEntries();

				Dictionary<string, RoomInfo> rooms;
				NetworkManager.Instance.RoomsGet(out rooms);

				int row = 0;
				foreach (var room in rooms)
				{
					// create the row Object
					GameObject entry = Instantiate(listEntryPrefab, transform);
					string occupancy = string.Format("[{0}/{1}]", room.Value.PlayerCount, room.Value.MaxPlayers);
					entry.transform.GetChild(0).GetComponent<TMP_Text>().text = room.Key;
					entry.transform.GetChild(1).GetComponent<TMP_Text>().text = occupancy;
					entry.GetComponent<RoomListEntry>().RoomName = room.Key;

					// alternate row colour
					Image background = entry.GetComponent<Image>();
					if (row % 2 == 0)
					{
						background.color = new Color(background.color.r, background.color.g, background.color.b, 0.4f);
					}
					else
					{
						background.color = new Color(background.color.r, background.color.g, background.color.b, 0.1f);
					}
					++row;
				}
			}
			// check for updates in the room list every 1 second
			yield return new WaitForSeconds(1);
		}
	}

	private void ClearRoomEntries()
	{
		for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
		{
			Destroy(gameObject.transform.GetChild(i).gameObject);
		}
	}

	public void DirtyRoomList()
	{
		roomListDirty = true;
	}
}
