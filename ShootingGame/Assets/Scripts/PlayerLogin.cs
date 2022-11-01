using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerLogin : MonoBehaviourPunCallbacks
{
	[SerializeField]
	TMP_Text playerName;

	public void OnSubmit()
	{
		Debug.Log(playerName.text);
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		base.OnConnectedToMaster();
		Debug.Log("Connection succesful!");
		SceneTransitionManager.Instance.LoadScene("MultiPlayer");
	}


}
