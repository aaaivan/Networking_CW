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

	void Update()
	{
		int children = transform.childCount;
		for (int i = 0; i < children; i++)
		{
			Image background = transform.GetChild(i).GetComponent<Image>();
			if (i % 2 == 0)
			{
				background.color = new Color(background.color.r, background.color.g, background.color.b, 0.4f);
			}
			else
			{
				background.color = new Color(background.color.r, background.color.g, background.color.b, 0.1f);
			}
		}
	}

	IEnumerator UpdateRoomList()
	{
		while (true)
		{
			if (roomListDirty)
			{
				Debug.Log("Updating room list");
				roomListDirty = false;
				ClearRoomEntries();

				List<RoomInfo> rooms;
				NetworkManager.Instance.RoomsGet(out rooms);

				foreach (var room in rooms)
				{
					GameObject entry = Instantiate(listEntryPrefab, transform);
					string roomName = room.Name;
					string occupancy = string.Format("[{0}/{1}]", room.PlayerCount, room.MaxPlayers);
					entry.transform.GetChild(0).GetComponent<TMP_Text>().text = roomName;
					entry.transform.GetChild(1).GetComponent<TMP_Text>().text = occupancy;
					entry.GetComponent<RoomListEntry>().RoomName = roomName;
				}
			}

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
