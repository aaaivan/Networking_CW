using Photon.Pun;
using System;
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
		MenuNavigationManager.Instance.ShowMenu("RoomList");
	}

	public void Disconnect()
	{
		NetworkManager.Instance.DisconnectFromMaster();
	}

	public void LeaveLobby()
	{
		NetworkManager.Instance.disconnectOnLeaveLobby = true;
		NetworkManager.Instance.LeaveLobby();
	}

	public void ShowRoomOptions()
	{
		MenuNavigationManager.Instance.ShowMenu("RoomOptions");
	}

	public void LeaveRoom()
	{
		NetworkManager.Instance.leavingRoom = true;
		NetworkManager.Instance.LeaveRoom();
	}

	public void StartMultiplayerGame()
	{
		var players = PhotonNetwork.PlayerList;
		List<string> playerNames = new List<string>();

		// make sure there aren't players with the same nickname
		foreach (var player in players)
		{
			if(playerNames.Contains(player.NickName))
			{
				return;
			}
			else
			{
				playerNames.Add(player.NickName);
			}
		}

		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.LoadLevel("MultiPlayer");
	}
}
