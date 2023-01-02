using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System;

public class OnlineChatManager : MonoBehaviour, IChatClientListener
{
	OnlineChatUI ChatUI
	{
		get { return MultiplayerLevelManager.Instance != null ?
				MultiplayerLevelManager.Instance.Chat :
				NetworkManager.Instance.Chat; }
	}

	ChatClient chatClient;
	public ChatClient ChatClient { get { return chatClient; } }

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

	public void OnConnected()
	{
		Debug.Log("Chat: Subscribed to Chat");
		ChatUI.SetSendButtonState(true);
		chatClient.Subscribe(PhotonNetwork.CurrentRoom.Name, creationOptions: new ChannelCreationOptions() { PublishSubscribers = true });
	}

	public void OnDisconnected()
	{
		ChatUI.SetSendButtonState(false);
		ChatUI.gameObject.SetActive(false);
		Debug.Log("Chat: Unsubscribed from Chat");
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		ChatChannel roomChat;
		if(chatClient.TryGetChannel(PhotonNetwork.CurrentRoom.Name, out roomChat))
		{
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

	public void SendChatMessage(string text)
	{
		if (text.Length == 0)
			return;

		chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, text);
	}
}
