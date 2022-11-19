using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	Button startGameBtn;
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

	public bool disconnectOnLeaveLobby = false;

	static NetworkManager instance;
	static public NetworkManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a NetworkManager to your scene");
			return instance;
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Awake()
	{
		instance = this;
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void ConnectToMaster(string id)
	{
		playerId = id;
		PhotonNetwork.LocalPlayer.NickName = playerId;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
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
		base.OnDisconnected(cause);
		Debug.Log("Disconnected from the master server (" + cause + ")");
		MenuNavigationManager.Instance.ShowMenu(1);
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
		base.OnCreatedRoom();
		Debug.Log("Room has been created!");
	}
	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		Debug.Log("Failed to create room (" + returnCode + ").");
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
		base.OnJoinedRoom();
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
		base.OnMasterClientSwitched(newMasterClient);

		if (startGameBtn != null)
			startGameBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		base.OnJoinRoomFailed(returnCode, message);
		Debug.Log("Failed to join the specified room");
		JoinLobby();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		base.OnJoinRandomFailed(returnCode, message);
		Debug.Log("Failed to join a random room");
		JoinLobby();
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
		Debug.Log("Room has been left.");
		MenuNavigationManager.Instance.ShowMenu(2);
		JoinLobby();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged.Invoke();
		}

		if (PhotonNetwork.PlayerList.Length > 1)
			startGameBtn.interactable = true;
		else
			startGameBtn.interactable = false;
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		if (OnPlayersChanged != null)
		{
			OnPlayersChanged.Invoke();
		}

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
		base.OnJoinedLobby();
		Debug.Log("Lobby joined"!);
		joinRandomRoomBtn.interactable = cachedRoomList.Count > 0;
	}

	public void LeaveLobby()
	{
		PhotonNetwork.LeaveLobby();
	}

	public override void OnLeftLobby()
	{
		base.OnLeftLobby();
		Debug.Log("Lobby left!");
		cachedRoomList.Clear();
		if(disconnectOnLeaveLobby)
			DisconnectFromMaster();

		disconnectOnLeaveLobby = false;
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
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
