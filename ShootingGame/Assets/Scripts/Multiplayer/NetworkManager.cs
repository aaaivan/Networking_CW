using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	Button startGameBtn;
	[SerializeField]
	TMP_Text roomNameTakenWarning;
	[SerializeField]
	Button joinRandomRoomBtn;

	string playerId;
	public string PlayerId
	{
		get { return playerId; }
	}

	public delegate void UpdatePlayersList();
	public static event UpdatePlayersList OnPlayersChanged;

	Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
	public delegate void UpdateRoomsList();
	public static event UpdateRoomsList OnRoomsChanged;

	[HideInInspector]
	public bool disconnectOnLeaveLobby = false;

	static NetworkManager instance;
	static public NetworkManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			PhotonNetwork.AutomaticallySyncScene = true;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void ConnectToMaster(string id)
	{
		playerId = id;
		PhotonNetwork.LocalPlayer.NickName = playerId;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log(playerId + " has connected to the master sever!");
		MenuNavigationManager.Instance.ShowMenu(2);
		JoinLobby();
	}

	public void DisconnectFromMaster()
	{
		playerId = "";
		PhotonNetwork.Disconnect();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("Disconnected from the master server (" + cause + ")");
		if (MenuNavigationManager.Instance != null)
		{
			MenuNavigationManager.Instance.ShowMenu(1);
		}
	}

	public void CreateRoom(string name)
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 4;
		roomOptions.IsVisible = true;

		PhotonNetwork.CreateRoom(name, roomOptions);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Room has been created!");
	}
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		Debug.Log("Failed to create room (" + returnCode + ").");
		roomNameTakenWarning.gameObject.SetActive(true);
		JoinLobby();
	}

	public void JoinRoom(string name)
	{
		PhotonNetwork.JoinRoom(name);
	}

	public void JoinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Room joined successfully!");
		MenuNavigationManager.Instance.ShowMenu(5);
		if (startGameBtn != null)
			startGameBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
		if (startGameBtn != null)
			startGameBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to join the specified room");
		JoinLobby();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to join a random room");
		JoinLobby();
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		Debug.Log("Room has been left.");
		MenuNavigationManager.Instance.ShowMenu(2);
		JoinLobby();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged.Invoke();
		}

		PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers;

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged.Invoke();
		}

		PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers;

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public void PlayersGet(out Player[] players)
	{
		players = PhotonNetwork.PlayerList;
	}

	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby();
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("Lobby joined"!);
		joinRandomRoomBtn.interactable = cachedRoomList.Count > 0;
	}

	public void LeaveLobby()
	{
		PhotonNetwork.LeaveLobby();
	}

	public override void OnLeftLobby()
	{
		Debug.Log("Lobby left!");
		cachedRoomList.Clear();
		if(disconnectOnLeaveLobby)
			DisconnectFromMaster();

		disconnectOnLeaveLobby = false;
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("Number of rooms: " + roomList.Count);

		foreach (RoomInfo room in roomList)
		{
			if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
			{
				if (cachedRoomList.ContainsKey(room.Name))
				{
					cachedRoomList.Remove(room.Name);
				}
			}
			else
			{
				cachedRoomList[room.Name] = room;
			}
		}

		joinRandomRoomBtn.interactable = cachedRoomList.Count > 0;
		if (OnRoomsChanged != null)
		{
			OnRoomsChanged.Invoke();
		}
	}

	public void RoomsGet(out Dictionary<string, RoomInfo> rooms)
	{
		rooms = cachedRoomList;
	}
}
