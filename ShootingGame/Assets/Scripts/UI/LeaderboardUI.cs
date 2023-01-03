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

	string entryText = "{0} - {1}";

	private void OnEnable()
	{
		foreach (Transform t in leaderboardContent)
		{
			Destroy(t.gameObject);
		}

		noScoreText.gameObject.SetActive(false);
		leaderboardScrollView.gameObject.SetActive(false);

		GlobalLeaderboard.Instance.GetLeaderboard();
	}

	private void OnDisable()
	{
		foreach(Transform t in leaderboardContent)
		{
			Destroy(t.gameObject);
		}
	}

	public void AddLeaderboardScore(int ranking, string player, int score)
	{
		noScoreText.gameObject.SetActive(false);
		leaderboardScrollView.gameObject.SetActive(true);

		GameObject go = Instantiate(leaderboardEntryPrefab, leaderboardContent);
		go.transform.Find("Key").GetComponent<TMP_Text>().text = string.Format(entryText, ranking, player);
		go.transform.Find("Value").GetComponent<TMP_Text>().text = score.ToString();

		if (ranking % 2 == 1)
		{
			Image image = go.GetComponent<Image>();
			image.color = new Color(0, 0, 0, 0);
		}
	}

	public void ShowNoScoreText()
	{
		noScoreText.gameObject.SetActive(true);
		leaderboardScrollView.gameObject.SetActive(false);
	}
}
