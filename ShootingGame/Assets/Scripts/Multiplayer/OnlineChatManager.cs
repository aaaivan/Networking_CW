using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class OnlineChatManager : MonoBehaviour, IChatClientListener
{
	[SerializeField]
	TMP_InputField messageInputField;
	[SerializeField]
	RectTransform chatContent;
	[SerializeField]
	Button sendMessageButton;
	[SerializeField]
	GameObject chatMessagePrefab;
	[SerializeField]
	GameObject userJoinedLeftPrefab;
	string lastSender = string.Empty;


	ChatClient chatClient;
	public ChatClient ChatClient { get { return chatClient; } }

	public OnlineChatManager()
	{
		chatClient = new ChatClient(this);
	}

	private void OnEnable()
	{
		sendMessageButton.interactable = false;
		lastSender = string.Empty;
	}

	private void OnDisable()
	{
		foreach(Transform t in chatContent)
		{
			Destroy(t.gameObject);
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
		sendMessageButton.interactable = true;
		chatClient.Subscribe(PhotonNetwork.CurrentRoom.Name, creationOptions: new ChannelCreationOptions() { PublishSubscribers = true });
	}

	public void OnDisconnected()
	{
		sendMessageButton.interactable = false;
		gameObject.SetActive(false);
		Debug.Log("Chat: Unsubscribed from Chat");
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		ChatChannel roomChat;
		if(chatClient.TryGetChannel(PhotonNetwork.CurrentRoom.Name, out roomChat))
		{
			for(int i = 0; i < messages.Length; i++)
			{
				GameObject go = Instantiate(chatMessagePrefab, chatContent);
				if(lastSender == senders[i])
				{
					go.transform.GetChild(0).gameObject.SetActive(false);
				}
				else
				{
					go.transform.GetChild(0).GetComponent<TMP_Text>().text = senders[i];
				}
				go.transform.GetChild(1).GetComponent<TMP_Text>().text = messages[i].ToString();
				LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());

				lastSender = senders[i];
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
		GameObject go = Instantiate(userJoinedLeftPrefab, chatContent);
		TMP_Text text = go.GetComponent<TMP_Text>();
		text.text = user + " has joined the room";
		LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
		lastSender = "";
	}

	public void OnUserUnsubscribed(string channel, string user)
	{
		Debug.Log("user unsubscribed");
		GameObject go = Instantiate(userJoinedLeftPrefab, chatContent);
		TMP_Text text = go.GetComponent<TMP_Text>();
		text.text = user + " has left the room";
		LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());
		lastSender = "";
	}

	public void SendChatMessage()
	{
		if (messageInputField.text.Length == 0)
			return;

		chatClient.PublishMessage(PhotonNetwork.CurrentRoom.Name, messageInputField.text);
		messageInputField.text = string.Empty;
	}
}
