using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class OnlineChatUI : MonoBehaviour
{
	[SerializeField]
	GameObject chatMessagePrefab;
	[SerializeField]
	GameObject userJoinedLeftPrefab;

	[SerializeField]
	TMP_InputField messageInputField;
	[SerializeField]
	RectTransform chatContent;
	public RectTransform ChatContent { get { return chatContent; } }
	[SerializeField]
	Button sendMessageButton;

	string lastSender = string.Empty;

	private void OnEnable()
	{
		ResetLastSender();
		bool activateChat = PhotonNetwork.CurrentRoom != null;
		sendMessageButton.interactable = activateChat;
	}

	private void OnDisable()
	{
		foreach (Transform t in chatContent)
		{
			Destroy(t.gameObject);
		}
	}

	public void AddMessageToChat(string sender, string message)
	{
		GameObject go = Instantiate(chatMessagePrefab, chatContent);
		if (lastSender == sender)
		{
			go.transform.GetChild(0).gameObject.SetActive(false);
		}
		else
		{
			go.transform.GetChild(0).GetComponent<TMP_Text>().text = sender;
		}
		go.transform.GetChild(1).GetComponent<TMP_Text>().text = message;
		LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());

		lastSender = sender;
	}

	public void OnPlayersInRoomChanged(string user, bool joined)
	{
		GameObject go = Instantiate(userJoinedLeftPrefab, chatContent);
		TMP_Text text = go.GetComponent<TMP_Text>();
		if(joined)
		{
			text.text = user + " has joined.";
		}
		else
		{
			text.text = user + " has left.";
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(go.GetComponent<RectTransform>());

		ResetLastSender();
	}

	public void SetSendButtonState(bool interactable)
	{
		sendMessageButton.interactable = interactable;
	}

	public void OnSendButtonClicked()
	{
		OnlineChatManager.Instance.SendChatMessage(PhotonNetwork.CurrentRoom.Name, messageInputField.text);
		messageInputField.text = string.Empty;
	}

	public void ResetLastSender()
	{
		lastSender = string.Empty;
	}
}
