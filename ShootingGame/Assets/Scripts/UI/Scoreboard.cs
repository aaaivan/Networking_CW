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
	string playerScoreMessage = "Kills {0}";
	[SerializeField]
	GameObject scoreEntryPrefab;
	List<GameObject> scoreList = new List<GameObject>();

	private void Awake()
	{
		foreach(var player in PhotonNetwork.PlayerList)
		{
			player.SetScore(0);
			var scroreEntry = Instantiate(scoreEntryPrefab, transform);
			scroreEntry.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
			scroreEntry.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format(playerScoreMessage, player.GetScore());
			scoreList.Add(scroreEntry);
		}
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

		List<Tuple<Photon.Realtime.Player, int>> players = new List<Tuple<Photon.Realtime.Player, int>>();
		foreach (var player in PhotonNetwork.PlayerList)
		{
			players.Add(new Tuple<Photon.Realtime.Player, int>(player, player.GetScore()));
		}
		players.Sort((a, b) => b.Item2.CompareTo(a.Item2));

		int maxScore = players[0].Item2;
		for (int i = 0; i < scoreList.Count; ++i)
		{
			Photon.Realtime.Player p = players[i].Item1;
			TMP_Text playerName = scoreList[i].transform.GetChild(0).GetComponent<TMP_Text>();
			TMP_Text playerScore = scoreList[i].transform.GetChild(1).GetComponent<TMP_Text>();
			playerName.text = p.NickName;
			playerScore.text = string.Format(playerScoreMessage, p.GetScore());
			if(p.GetScore() == maxScore)
			{
				playerName.color = new Color(1.0f,0.592f, 0.192f);
				playerScore.color = new Color(1.0f, 0.592f, 0.192f);
			}
			else
			{
				playerName.color = Color.white;
				playerScore.color = Color.white;
			}
		}
	}
}
