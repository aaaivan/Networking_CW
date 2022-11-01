using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomCreationButton : MonoBehaviour
{
	[SerializeField]
	TMP_Text roomName;

	public void OnSubmit()
	{
		if (roomName.text.Length > 1)
		{
			NetworkManager.Instance.CreateRoom(roomName.text);
		}
		roomName.text = "";
	}
}
