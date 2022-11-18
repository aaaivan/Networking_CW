using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
		base.OnPlayerLeftRoom(otherPlayer);

		if (MultiplayerLevelManager.Instance.playersMap.ContainsKey(otherPlayer))
		{
			UpdateScoreboard(MultiplayerLevelManager.Instance.playersMap[otherPlayer]);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

		if(MultiplayerLevelManager.Instance.playersMap.ContainsKey(targetPlayer))
		{
			UpdateScoreboard(MultiplayerLevelManager.Instance.playersMap[targetPlayer]);
		}
	}

	public void UpdateScoreboard(PlayerMechanics _player)
	{
		List<Player> players = new List<Player>();
		foreach(var p in PhotonNetwork.PlayerList)
		{
			if(MultiplayerLevelManager.Instance.playersMap.ContainsKey(p))
			{
				players.Add(p);
			}
		}

		if(players.Count == 0)
			return;

		players.Sort((a, b) => PlayerEntriesCompare(a, b));

		int maxScore = players[0].GetScore();
		int minDeaths = MultiplayerLevelManager.Instance.playersMap[players[0]].Deaths;

		for (int i = 0; i < scoreList.Count; ++i)
		{
			if(i < players.Count)
			{
				scoreList[i].SetActive(true);
				Player p = players[i];
				int deaths = MultiplayerLevelManager.Instance.playersMap[p].Deaths;
				TMP_Text playerName = scoreList[i].transform.GetChild(0).GetComponent<TMP_Text>();
				TMP_Text playerScore = scoreList[i].transform.GetChild(1).GetComponent<TMP_Text>();
				playerName.text = p.NickName;
				playerScore.text = string.Format(playerScoreMessage, p.GetScore(), deaths);

				// highlight winner
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
			if (p_a.Deaths < p_b.Deaths)
			{
				return -1;
			}
			else if (p_a.Deaths > p_b.Deaths)
			{
				return 1;
			}
		}
		return 0;
	}
}
