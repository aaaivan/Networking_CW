using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomListEntry : MonoBehaviour
{
	string roomName = "";
	public string RoomName
	{
		get { return roomName; }
		set { roomName = value; }
	}

	public void JoinRoom()
	{
		NetworkManager networkManager = NetworkManager.Instance;
		networkManager.JoinRoom(roomName);
	}
}
