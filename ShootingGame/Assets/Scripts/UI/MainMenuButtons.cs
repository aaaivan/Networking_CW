using Photon.Pun;
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
		MenuNavigationManager.Instance.ShowMenu(4);
	}

	public void Disconnect()
	{
		NetworkManager.Instance.DisconnectFromMaster();
	}

	public void LeaveLobby()
	{
		NetworkManager.Instance.LeaveLobby();
	}

	public void ShowRoomOptions()
	{
		MenuNavigationManager.Instance.ShowMenu(2);
	}

	public void LeaveRoom()
	{
		NetworkManager.Instance.LeaveRoom();
	}
	public void StartMultiplayerGame()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.CurrentRoom.IsVisible = false;
		SceneTransitionManager.Instance.LoadScene("MultiPlayer");
	}
}
