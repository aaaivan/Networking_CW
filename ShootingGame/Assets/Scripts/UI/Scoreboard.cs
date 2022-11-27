using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Multiplayer scoreboard panel
/// </summary>
public class Scoreboard : MonoBehaviourPunCallbacks
{
	[SerializeField]
	TMP_Text timerField;
	[SerializeField]
	string timerString = "Time: {0}";
	int currentTime = 0;

	[SerializeField]
	string playerScoreMessage = "Kills {0} (Deaths: {1})";
	[SerializeField]
	GameObject scoreEntryPrefab;
	List<GameObject> scoreList = new List<GameObject>();

	public override void OnEnable()
	{
		base.OnEnable();
		PlayerMechanics.OnPlayerKilled += UpdateScoreboard;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PlayerMechanics.OnPlayerKilled -= UpdateScoreboard;
	}

	private void Awake()
	{
		// add each player to the scoreboard
		foreach(var player in PhotonNetwork.PlayerList)
		{
			player.SetScore(0);
			GameObject scroreEntry = Instantiate(scoreEntryPrefab, transform);
			scroreEntry.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
			scroreEntry.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format(playerScoreMessage, player.GetScore(), 0);
			scoreList.Add(scroreEntry);
		}
	}

	private void Update()
	{
		// update the timer
		int time = (int)MultiplayerLevelManager.Instance.TimeLeft;
		if (time != currentTime)
		{
			currentTime = time;
			string timeStr = string.Format(timerString, currentTime);
			timerField.text = timeStr;
		}
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		// a player left the room: update the scoreboard so that they get removed
		if (MultiplayerLevelManager.Instance.playersMap.ContainsKey(otherPlayer))
		{
			UpdateScoreboard(MultiplayerLevelManager.Instance.playersMap[otherPlayer]);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		// check whether the score is among the changed properties
		// if yes update the scoreboard
		if(changedProps.ContainsKey(PunPlayerScores.PlayerScoreProp) &&
		   MultiplayerLevelManager.Instance.playersMap.ContainsKey(targetPlayer))
		{
			UpdateScoreboard(MultiplayerLevelManager.Instance.playersMap[targetPlayer]);
		}
	}

	/// <summary>
	/// re-build the scoreboard
	/// </summary>
	/// <param name="_player"> player whose score has changed (unused param in this implementation) </param>
	public void UpdateScoreboard(PlayerMechanics _player)
	{
		// get list of online players in the room
		List<Player> players = new List<Player>();
		foreach(var p in PhotonNetwork.PlayerList)
		{
			// check if the player is in the player map. This should always be the case.
			if(MultiplayerLevelManager.Instance.playersMap.ContainsKey(p))
			{
				players.Add(p);
			}
		}

		if(players.Count == 0)
			return;

		// sort the players from highest to lowest store
		players.Sort((a, b) => PlayerEntriesCompare(a, b));

		int maxScore = players[0].GetScore(); // score of the winning player
		int minDeaths = MultiplayerLevelManager.Instance.playersMap[players[0]].DeathCount; // death count of the winning player

		for (int i = 0; i < scoreList.Count; ++i)
		{
			if(i < players.Count)
			{
				// update player stats
				scoreList[i].SetActive(true);
				Player p = players[i];
				int deaths = MultiplayerLevelManager.Instance.playersMap[p].DeathCount;
				TMP_Text playerName = scoreList[i].transform.GetChild(0).GetComponent<TMP_Text>();
				TMP_Text playerScore = scoreList[i].transform.GetChild(1).GetComponent<TMP_Text>();
				playerName.text = p.NickName;
				playerScore.text = string.Format(playerScoreMessage, p.GetScore(), deaths);

				// highlight winner(s)
				if (p.GetScore() == maxScore && deaths == minDeaths)
				{
					playerName.color = new Color(1.0f, 0.592f, 0.192f);
					playerScore.color = new Color(1.0f, 0.592f, 0.192f);
				}
				else
				{
					playerName.color = Color.white;
					playerScore.color = Color.white;
				}
			}
			else
			{
				scoreList[i].SetActive(false);
			}
		}
	}

	/// <summary>
	/// Compare two players. Player are ordered by increasing number of kills first
	/// players with the same number of kills are ordered by increasing death count.
	/// </summary>
	/// <param name="a">lhs player</param>
	/// <param name="b">rhs player</param>
	/// <returns> -1 if b comes before b, 1 if a comes before b, 0 otherwise</returns>
	private int PlayerEntriesCompare(Photon.Realtime.Player a, Photon.Realtime.Player b)
	{
		PlayerMechanics p_a = MultiplayerLevelManager.Instance.playersMap[a];
		PlayerMechanics p_b = MultiplayerLevelManager.Instance.playersMap[b];

		if (a.GetScore() > b.GetScore())
		{
			return -1;
		}
		else if (a.GetScore() < b.GetScore())
		{
			return 1;
		}
		else
		{
			if (p_a.DeathCount < p_b.DeathCount)
			{
				return -1;
			}
			else if (p_a.DeathCount > p_b.DeathCount)
			{
				return 1;
			}
		}
		return 0;
	}
}
