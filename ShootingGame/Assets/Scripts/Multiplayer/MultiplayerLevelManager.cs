using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	int maxKills = 0;
	[SerializeField]
	GameObject gameOverScreen = null;
	[SerializeField]
	GameObject scoreboard = null;
	[SerializeField]
	TMP_Text winnerText = null;


	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);

		if (PhotonNetwork.PlayerList.Length == 1)
		{
			winnerText.text = PhotonNetwork.PlayerList[0].NickName;
			gameOverScreen.SetActive(true);
			scoreboard.SetActive(false);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

		if(targetPlayer.GetScore() >= maxKills)
		{
			winnerText.text = targetPlayer.NickName;
			gameOverScreen.SetActive(true);
			scoreboard.SetActive(false);

			ThirdPersonController fpc = InputsManager.Instance.thirdPersonController;
			GameObject player = fpc.gameObject;
			StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
			if (Input.mousePresent)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				inputs.cursorLocked = false;
				inputs.cursorInputForLook = false;
			}
			fpc.DisableGameInputs();
		}
	}

	public void LeaveGame()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		base.OnLeftRoom();
		PhotonNetwork.Disconnect();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		SceneTransitionManager.Instance.LoadScene("Main");
	}
}
