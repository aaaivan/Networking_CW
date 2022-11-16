using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetRoomName : MonoBehaviour
{
	[SerializeField]
	TMP_Text roomNameField;

	private void OnEnable()
	{
		roomNameField.text = PhotonNetwork.CurrentRoom.Name;
	}

	private void OnDisable()
	{
		roomNameField.text = "";
	}
}
