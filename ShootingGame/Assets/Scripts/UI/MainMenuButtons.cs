using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
	public void JoinRandom()
	{
		NetworkManager.Instance.JoinRandomRoom();
	}

	public void ShowRooms()
	{
		NetworkManager.Instance.JoinLobby();
	}

	public void Disconnect()
	{
		NetworkManager.Instance.DisconnectFromMaster();
	}

	public void LeaveLobby()
	{
		NetworkManager.Instance.LeaveLobby();
	}

	public void LeaveRoom()
	{
		NetworkManager.Instance.LeaveRoom();
	}
}
