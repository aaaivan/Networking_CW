using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class GlobalLeaderboard : MonoBehaviour
{
	[SerializeField]
	int maxEntries = 5;
	[SerializeField]
	string statisticName = "Most Kills";

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

	public void SubmitScore(int playerScore)
	{
		UpdatePlayerStatisticsRequest request = new UpdatePlayerStatisticsRequest()
		{
			Statistics = new List<StatisticUpdate>
			{
				new StatisticUpdate()
				{
					StatisticName = statisticName,
					Value = playerScore,
				}
			}
		};

		PlayFabClientAPI.UpdatePlayerStatistics(request, PlayFabUpdateStatsResult, PlayFabUpdateStatsError);
	}

	void PlayFabUpdateStatsResult(UpdatePlayerStatisticsResult result)
	{
		Debug.Log("PlayFab - Score Submitted");
	}

	void PlayFabUpdateStatsError(PlayFabError error)
	{
		Debug.Log("PlayFab Error: " + error);
	}

	public void GetLeaderboard()
	{
		GetLeaderboardRequest request = new GetLeaderboardRequest()
		{
			MaxResultsCount = maxEntries,
			StatisticName = statisticName,
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
			NetworkManager.Instance.Leaderboard.AddLeaderboardScore(i + 1, entry.PlayFabId, entry.StatValue);
		}
	}

	public void PlayFabGetLeaderboardError(PlayFabError error)
	{
		Debug.Log("PlayFab Error: " + error);
	}
}
