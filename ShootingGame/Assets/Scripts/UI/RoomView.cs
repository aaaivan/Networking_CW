using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomView : MonoBehaviour
{
	[SerializeField]
	TMP_Text roomNameField;

	private void OnEnable()
	{
		roomNameField.text = PhotonNetwork.CurrentRoom.Name;
		NetworkManager.Instance.Chat.gameObject.SetActive(true);
	}

	private void OnDisable()
	{
		roomNameField.text = "";
	}
}
