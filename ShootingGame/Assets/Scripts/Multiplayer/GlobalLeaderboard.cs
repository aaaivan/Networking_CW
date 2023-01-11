using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public class GlobalLeaderboard : MonoBehaviour
{
	int maxEntries = 10;
	public readonly string totalKillsName = "TotalKills";
	public readonly string scoreName = "Score";

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

	public void UpdateLeaderboard(int totalKills, int score)
	{
		UpdatePlayerStatisticsRequest request1 = new UpdatePlayerStatisticsRequest()
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate()
				{
					StatisticName = totalKillsName,
					Value = totalKills,
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
					StatisticName = scoreName,
					Value = score,
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
			StatisticName = scoreName,
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

		for(int i = 0; i < entries.Count; )
		{
			int score = entries[i].StatValue;
			List<PlayerLeaderboardEntry> tier = new List<PlayerLeaderboardEntry>();
			Dictionary<PlayerLeaderboardEntry, int> dict = new Dictionary<PlayerLeaderboardEntry, int>();
			while (i < entries.Count && entries[i].StatValue == score)
			{
				List<StatisticModel> stats = entries[i].Profile.Statistics;
				int totalKillsIndex = stats.FindIndex((x) => x.Name == totalKillsName);

				tier.Add(entries[i]);
				dict.Add(entries[i], totalKillsIndex >= 0 ? entries[i].Profile.Statistics[totalKillsIndex].Value : 0);
				++i;
			}

			// sort the players with the same best score based on number of deaths
			tier.Sort((a, b) => dict[b].CompareTo(dict[a]));

			foreach(var entry in tier)
			{
				string playerId = entry.PlayFabId;

				NetworkManager.Instance.Leaderboard.AddLeaderboardScore(entry.Position + 1,
					entry.DisplayName,
					dict[entry],
					entry.StatValue, playerId == GameManager.Instance.PlayFabPlayerID);
			}
		}
	}

	public void PlayFabGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("PlayFab Error: " + error);
	}
}
