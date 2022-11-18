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
	MultiplayerGameOver gameOverScreen = null;
	[SerializeField]
	Scoreboard scoreboard = null;
	[SerializeField]
	float gameDuration = 120.0f;
	float timeLeft;
	bool playing = false;
	public float TimeLeft { get { return timeLeft; } }

	public Dictionary<Player, PlayerMechanics> playersMap = new Dictionary<Player, PlayerMechanics>();

	static MultiplayerLevelManager instance;
	public static MultiplayerLevelManager Instance
	{
		get { return instance; }
	}

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			timeLeft = gameDuration;
			playing = true;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void RegisterPlayer(Player photonPlayer, PlayerMechanics gameObjPlayer)
	{
		if (!playersMap.ContainsKey(photonPlayer))
		{
			playersMap.Add(photonPlayer, gameObjPlayer);
			scoreboard.UpdateScoreboard(gameObjPlayer);
		}
	}

	private void Update()
	{
		if(playing)
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0)
			{
				playing = false;

				List<string> winners;
				ComputeWinners(out winners);
				EndGame(winners);
			}
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);

		if (PhotonNetwork.PlayerList.Length == 1 && playing)
		{
			List<string> winners;
			ComputeWinners(out winners);
			EndGame(winners);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

		if(targetPlayer.GetScore() >= maxKills)
		{
			List<string> winners;
			ComputeWinners(out winners); 
			EndGame(winners);
		}
	}

	private void ComputeWinners(out List<string> winners)
	{
		winners = new List<string>();
		int maxScore = 0;
		int minDeaths = int.MaxValue;

		foreach (var player in PhotonNetwork.PlayerList)
		{
			if (player.GetScore() > maxScore)
			{
				minDeaths = playersMap[player].Deaths;
				maxScore = player.GetScore();

				winners.Clear();
				winners.Add(player.NickName);
			}
			else if (player.GetScore() == maxScore)
			{
				int deaths = playersMap[player].Deaths;
				if (deaths < minDeaths)
				{
					minDeaths = deaths;

					winners.Clear();
					winners.Add(player.NickName);
				}
				else if (deaths == minDeaths)
				{
					winners.Add(player.NickName);
				}
			}
		}
	}

	private void EndGame(List<string> winners)
	{
		gameOverScreen.SetWinners(winners);
		gameOverScreen.gameObject.SetActive(true);
		scoreboard.gameObject.SetActive(false);

		// Disable players controls and show the mouse cursor
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
