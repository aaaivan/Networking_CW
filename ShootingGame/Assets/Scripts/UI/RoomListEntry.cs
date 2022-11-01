using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RoomListEntry : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	Image button = null;
	string roomName = "";
	public string RoomName
	{
		get { return roomName; }
		set { roomName = value; }
	}

	static RoomListEntry selectedRoom = null;
	public static RoomListEntry SelectedRoom { get { return selectedRoom; } }

	private void Awake()
	{
		button = GetComponent<Image>();
	}

	public void OnSelect(BaseEventData eventData)
	{
		button.tintColor = new Color(button.tintColor.r, button.tintColor.g, button.tintColor.b, 1);
		selectedRoom = this;
	}

	public void OnDeselect(BaseEventData eventData)
	{
		button.tintColor = new Color(button.tintColor.r, button.tintColor.g, button.tintColor.b, 0);
		if(selectedRoom == this)
		{
			selectedRoom = null;
		}
	}

}
