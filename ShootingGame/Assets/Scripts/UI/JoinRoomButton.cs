using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomButton : MonoBehaviour
{
    public void JoinSelectedRoom()
	{
		RoomListEntry selectedRoom = RoomListEntry.SelectedRoom;
		if(selectedRoom != null)
		{
			NetworkManager networkManager = NetworkManager.Instance;
			networkManager.LeaveLobby();
			networkManager.JoinRoom(selectedRoom.RoomName);
		}
	}
}
