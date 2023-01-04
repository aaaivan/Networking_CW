using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class GlobalLeaderboard : MonoBehaviour
{
	int maxEntries = 10;
	public readonly string mostKillsName = "Most Kills";
	public readonly string totalGamesName = "Total Games";

	Dictionary<string, int> totalGamesDict = new Dictionary<string, int>();


	static GlobalLeaderboard instance;
	public static GlobalLeaderboard Instance
	{
		get { return instance; }
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void UpdateLeaderboard()
	{
		PlayerData playerData = GameManager.Instance.PlayerData;

		UpdatePlayerStatisticsRequest request1 = new UpdatePlayerStatisticsRequest()
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate()
				{
					StatisticName = mostKillsName,
					Value = playerData.totalKills,
				}
			}
		};
		PlayFabClientAPI.UpdatePlayerStatistics(request1,
			(result) => { Debug.Log("PlayFab - Score Submitted"); },
			(error) => { Debug.Log("PlayFab Error: " + error); });

		UpdatePlayerStatisticsRequest request2 = new UpdatePlayerStatisticsRequest()
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate()
				{
					StatisticName = totalGamesName,
					Value = playerData.totalGames,
				}
			}
		};
		PlayFabClientAPI.UpdatePlayerStatistics(request2,
			(result) => { Debug.Log("PlayFab - Score Submitted"); },
			(error) => { Debug.Log("PlayFab Error: " + error); });
	}

	public void GetLeaderboards()
	{
		Action<GetLeaderboardResult> sucessCallback = (result) =>
		{
			GetLeaderboardRequest request = new GetLeaderboardRequest()
			{
				MaxResultsCount = maxEntries,
				StatisticName = mostKillsName,
			};
			PlayFabClientAPI.GetLeaderboard(request, PlayFabGetLeaderboardResult, PlayFabGetLeaderboardError);

			List<PlayerLeaderboardEntry> entries = result.Leaderboard;
			foreach(var e in entries)
			{
				totalGamesDict.Add(e.PlayFabId, e.StatValue);
			}
		};

		GetLeaderboardRequest request = new GetLeaderboardRequest()
		{
			MaxResultsCount = maxEntries,
			StatisticName = totalGamesName,
		};
		PlayFabClientAPI.GetLeaderboard(request, sucessCallback, PlayFabGetLeaderboardError);
	}

	public void PlayFabGetLeaderboardResult(GetLeaderboardResult result)
	{
		Debug.Log("PlayFab - Leaderboard fetched succesfully");
		List<PlayerLeaderboardEntry> entries = result.Leaderboard;

		if(entries.Count == 0)
		{
			NetworkManager.Instance.Leaderboard.ShowNoScoreText();
			return;
		}

		for(int i = 0; i < entries.Count; ++i)
		{
			PlayerLeaderboardEntry entry = entries[i];
			string playerId = entry.PlayFabId;
			if(totalGamesDict.ContainsKey(playerId))
			{
				NetworkManager.Instance.Leaderboard.AddLeaderboardScore(entry.Position + 1, entry.DisplayName, totalGamesDict[playerId], entry.StatValue);
			}
		}

		totalGamesDict.Clear();
	}

	public void PlayFabGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("PlayFab Error: " + error);
		totalGamesDict.Clear();
	}
}
