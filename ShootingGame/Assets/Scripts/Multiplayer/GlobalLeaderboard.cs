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

	/// <summary>
	/// Fetch the players with highest number of total kills.
	/// Request for all statistics and the display name
	/// to be included in the result callback.
	/// </summary>
	public void GetLeaderboards()
	{
		GetLeaderboardRequest request = new GetLeaderboardRequest()
		{
			MaxResultsCount = maxEntries,
			StatisticName = mostKillsName,
			ProfileConstraints = new PlayerProfileViewConstraints()
			{
				ShowStatistics = true,
				ShowDisplayName = true,
			},
		};
		PlayFabClientAPI.GetLeaderboard(request, PlayFabGetLeaderboardResult, PlayFabGetLeaderboardError);
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
			List<StatisticModel> stats = entry.Profile.Statistics;
			int totalGamesIndex = stats.FindIndex((x) => x.Name == totalGamesName);

			NetworkManager.Instance.Leaderboard.AddLeaderboardScore(entry.Position + 1, entry.DisplayName, stats[totalGamesIndex].Value, entry.StatValue, playerId == GameManager.Instance.PlayFabPlayerID);
		}
	}

	public void PlayFabGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("PlayFab Error: " + error);
	}
}
