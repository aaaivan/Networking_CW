using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;

public class OnlineChatManager : MonoBehaviour, IChatClientListener
{
	OnlineChatUI ChatUI
	{
		get { return MultiplayerLevelManager.Instance != null ?
				MultiplayerLevelManager.Instance.Chat : // In-game chat
				NetworkManager.Instance.Chat; } // Main menu chat
	}

	ChatClient chatClient;

	static OnlineChatManager instance;
	public static OnlineChatManager Instance { get { return instance; } }

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			chatClient = new ChatClient(this);
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		chatClient.Service();
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		Debug.Log("Chat - " + level + ": " + message);
	}

	public void OnChatStateChange(ChatState state)
	{
		Debug.Log("Chat - OnStateChange: " + state);
	}

	/// <summary>
	/// Connect the local player to the online chat
	/// </summary>
	public void ConnectToChat()
	{
		var authenticationValues = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);
		chatClient.Connect(	PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
							"1.0",
							authenticationValues);
	}

	public void OnConnected()
	{
		Debug.Log("Chat: Subscribed to Chat");
	}

	/// <summary>
	/// Disconnect the local player from the online chat
	/// </summary>
	public void DisconnectFromChat()
	{
		chatClient.Disconnect();
	}

	public void OnDisconnected()
	{
		ChatUI.SetSendButtonState(false);
		ChatUI.gameObject.SetActive(false);
		Debug.Log("Chat: Unsubscribed from Chat");
	}

	/// <summary>
	/// Allow the player to send and receive messages in the specified channel
	/// </summary>
	/// <param name="name"> Name of the channel </param>
	public void SubscribeToChannel(string name)
	{
		ChatUI.SetSendButtonState(true);
		chatClient.Subscribe(name, creationOptions: new ChannelCreationOptions() { PublishSubscribers = true });
	}

	/// <summary>
	/// Prevent the player from sending and receiving messages in the specified channel
	/// </summary>
	/// <param name="name"> Name of the channel </param>
	public void UnsubscribeFromChannel(string name)
	{
		ChatUI.SetSendButtonState(false);
		ChatUI.gameObject.SetActive(false);
		chatClient.Unsubscribe(new string[] { name });
	}

	/// <summary>
	/// Callback function for incoming messages.
	/// </summary>
	/// <param name="channelName"> Channel the messages are directed to </param>
	/// <param name="senders"> List of senders. </param>
	/// <param name="messages"> List of messages. </param>
	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		ChatChannel roomChat;
		// check whether the player is subscribed to destination channel
		if(chatClient.TryGetChannel(PhotonNetwork.CurrentRoom.Name, out roomChat))
		{
			// send each message to the chatUI so that it can be displayed
			for(int i = 0; i < messages.Length; i++)
			{
				ChatUI.AddMessageToChat(senders[i], messages[i].ToString());
			}
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
	}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
	}

	public void OnUnsubscribed(string[] channels)
	{
	}

	public void OnUserSubscribed(string channel, string user)
	{
		Debug.Log("user subscribed");
		ChatUI.OnPlayersInRoomChanged(user, true);
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		Debug.Log("user unsubscribed");
		ChatUI.OnPlayersInRoomChanged(user, false);
	}

	/// <summary>
	/// Send a message to the specified channel.
	/// </summary>
	/// <param name="channel"> Name of the channel the message is directed to </param>
	/// <param name="text"> Message body </param>
	public void SendChatMessage(string channel, string text)
	{
		if (text.Length == 0)
			return;

		chatClient.PublishMessage(channel, text);
	}
}
