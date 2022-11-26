using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLoginButton : MonoBehaviour
{
	[SerializeField]
	TMP_Text playerName;

	public void OnSubmit()
	{
		if(playerName.text.Length > 0)
		{
			NetworkManager.Instance.ConnectToMaster(playerName.text);
		}
		playerName.text = "";
	}
}
