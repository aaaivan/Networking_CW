using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	int maxKills = 3; // num of kills to win the game
	[SerializeField]
	float gameDuration = 120.0f; // in seconds
	float startTime = 0;
	public float TimeLeft { get { return startTime + gameDuration - Time.time; } }
	bool playing = false;
	public bool Playing { get { return playing; } }

	[SerializeField]
	OnlineChatUI chat;
	public OnlineChatUI Chat { get { return chat; } }

	PhotonView phView;

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
			startTime = Time.time;
			playing = true;
			phView = GetComponent<PhotonView>();

			ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
			hash[RematchScreen.readyToRematchKey] = false;
			hash[PunPlayerScores.PlayerScoreProp] = 0;
			PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Add an online player to the dictionary that maps each Photon.Realtime.Player to the PlayerMechanics component used by it
	/// </summary>
	/// <param name="photonPlayer"> online player </param>
	/// <param name="gameObjPlayer"> PlayerMechanics component attached to the character controlled by the opnline player </param>
	public void RegisterPlayer(Player photonPlayer, PlayerMechanics gameObjPlayer)
	{
		if (!playersMap.ContainsKey(photonPlayer))
		{
			playersMap.Add(photonPlayer, gameObjPlayer);
			Scoreboard scoreboard = MenuNavigationManager.Instance.MenuGet(0).GetComponent<Scoreboard>();
			scoreboard.UpdateScoreboard(gameObjPlayer);
		}
	}

	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape) && playing)
		{
			InputsManager.Instance.ToggleThirdPersonInputs();
		}

		// check who won if the time runs out
		if (playing && TimeLeft < 0)
		{
			List<string> winners;
			ComputeWinners(out winners);
			EndGame(winners);
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// if there is only one player left in the room, end the game
		if (PhotonNetwork.PlayerList.Length == 1 && playing)
		{
			List<string> winners;
			ComputeWinners(out winners);
			EndGame(winners);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		// if a player has maxKills, end the game
		if(changedProps.ContainsKey(PunPlayerScores.PlayerScoreProp) && targetPlayer.GetScore() >= maxKills)
		{
			List<string> winners;
			ComputeWinners(out winners); 
			EndGame(winners);
			StorePersonalBest();
		}
	}

	void StorePersonalBest()
	{
		int score = PhotonNetwork.LocalPlayer.GetScore();
		PlayerData playerData = GameManager.Instance.PlayerData;
		if(score > playerData.bestScore)
		{
			playerData.username = PhotonNetwork.LocalPlayer.NickName;
			playerData.bestScore = score;
			playerData.bestScoreDate = DateTime.UtcNow.ToString();
			playerData.playersInGame = PhotonNetwork.PlayerList.Length;
			playerData.roomName = PhotonNetwork.CurrentRoom.Name;

			GlobalLeaderboard.Instance.SubmitScore(score);
			GameManager.Instance.SavePlayerData();
		}
	}

	/// <summary>
	/// Check who won the game. The winner is the player with the most kills.
	/// In the case of a tie, the player with the lowest death count wins.
	/// </summary>
	/// <param name="winners"> list populated with the winners </param>
	private void ComputeWinners(out List<string> winners)
	{
		winners = new List<string>();
		int maxScore = 0;
		int minDeaths = int.MaxValue;

		foreach (var player in PhotonNetwork.PlayerList)
		{
			if (player.GetScore() > maxScore)
			{
				minDeaths = playersMap[player].DeathCount;
				maxScore = player.GetScore();

				winners.Clear();
				winners.Add(player.NickName);
			}
			else if (player.GetScore() == maxScore)
			{
				int deaths = playersMap[player].DeathCount;
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

	/// <summary>
	/// End the game and display the game over screen with the winners.
	/// Also, disable the third person controls.
	/// </summary>
	/// <param name="winners">winners whoose nicknames are displayed on the game over screen</param>
	private void EndGame(List<string> winners)
	{
		if (!playing)
			return;

		playing = false;

		MultiplayerGameOver gameOverScreen = MenuNavigationManager.Instance.MenuGet(1).GetComponent<MultiplayerGameOver>();
		gameOverScreen.SetWinners(winners);
		MenuNavigationManager.Instance.ShowMenu(1);

		// Disable players controls and show the mouse cursor
		InputsManager.Instance.DisableThirdPersonInputs();
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
		playing = false;

		// Disconnect the local player from the online chat
		OnlineChatManager.Instance.ChatClient.Disconnect();
		chat.gameObject.SetActive(false);

		if (cause == DisconnectCause.DisconnectByClientLogic)
		{
			SceneManager.LoadScene("Main");
		}
		else // if we disconnected because of a network issue, show the disconnection message
		{
			MenuNavigationManager.Instance.ShowMenu(2);
			InputsManager.Instance.DisableThirdPersonInputs();
		}
	}

	public void QuitGameOffline()
	{
		SceneManager.LoadScene("Main");
	}

	public void StartRematch()
	{
		phView.RPC("QuitIfNotReadyAndStart", RpcTarget.AllViaServer);
	}

	[PunRPC]
	private void QuitIfNotReadyAndStart()
	{
		PhotonNetwork.LocalPlayer.CustomProperties[PunPlayerScores.PlayerScoreProp] = 0;
		bool ready = false;
		// check whether the local player has the "readyToRematch" custom property set to true
		if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(RematchScreen.readyToRematchKey))
		{
			ready = (bool)PhotonNetwork.LocalPlayer.CustomProperties[RematchScreen.readyToRematchKey];
		}
		if (ready) // if player is ready to rematch, reload the multiplayer level
		{
			// This needs to be called on all clients that will play another game, and not only the master client
			// because PhotonNetwork.LoadLevel does not synchronise across all player when re-loading the current scene
			PhotonNetwork.IsMessageQueueRunning = false;
			PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			// Kick players that are not ready to rematch out of the room
			LeaveGame();
		}
	}

	/// <summary>
	/// Set the player as ready to rematch through custom properties
	/// </summary>
	public void SetReadyForRematch()
	{
		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
		hash[RematchScreen.readyToRematchKey] = true;
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		MenuNavigationManager.Instance.ShowMenu(3);
	}
}
