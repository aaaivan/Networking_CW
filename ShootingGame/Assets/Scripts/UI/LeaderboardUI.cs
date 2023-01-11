using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
	[SerializeField]
	GameObject leaderboardEntryPrefab;
	[SerializeField]
	RectTransform leaderboardContent;
	[SerializeField]
	RectTransform leaderboardScrollView;
	[SerializeField]
	RectTransform noScoreText;
	[SerializeField]
	RectTransform loadingText;

	string entryText = "{0} - {1}";

	private void OnEnable()
	{
		foreach (Transform t in leaderboardContent)
		{
			Destroy(t.gameObject);
		}

		noScoreText.gameObject.SetActive(false);
		leaderboardScrollView.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(true);

		GlobalLeaderboard.Instance.GetLeaderboards();
	}

	private void OnDisable()
	{
		foreach(Transform t in leaderboardContent)
		{
			Destroy(t.gameObject);
		}
	}

	public void AddLeaderboardScore(int ranking, string player, int totKills, int score, bool localPlayer)
	{
		noScoreText.gameObject.SetActive(false);
		leaderboardScrollView.gameObject.SetActive(true);
		loadingText.gameObject.SetActive(false);

		GameObject go = Instantiate(leaderboardEntryPrefab, leaderboardContent);
		go.transform.Find("Player").GetComponent<TMP_Text>().text = string.Format(entryText, ranking, localPlayer ? "YOU" : player);
		go.transform.Find("Kills").GetComponent<TMP_Text>().text = totKills.ToString();
		go.transform.Find("Score").GetComponent<TMP_Text>().text = score.ToString();

		if(localPlayer)
		{
			Image image = go.GetComponent<Image>();
			image.color = new Color(0, 1, 0.5f, 0.1f);
		}
		else if (ranking % 2 == 1)
		{
			Image image = go.GetComponent<Image>();
			image.color = new Color(0, 0, 0, 0);
		}
	}

	public void ShowNoScoreText()
	{
		noScoreText.gameObject.SetActive(true);
		leaderboardScrollView.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(false);
	}
}
