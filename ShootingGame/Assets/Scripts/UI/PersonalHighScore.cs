using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalHighScore : MonoBehaviour
{
	[SerializeField]
	RectTransform noScorePanel;
	[SerializeField]
	RectTransform statsPanel;
	RectTransform content;

	[SerializeField]
	GameObject personalHighScorePrefab;

	private void Awake()
	{
		content = statsPanel.Find("Viewport/Content").GetComponent<RectTransform>();
	}

	private void OnEnable()
	{
		PlayerData playerData = GameManager.Instance.PlayerData;
		if(playerData.username == null)
		{
			statsPanel.gameObject.SetActive(false);
			noScorePanel.gameObject.SetActive(true);
		}
		else
		{
			statsPanel.gameObject.SetActive(true);
			noScorePanel.gameObject.SetActive(false);

			// rebuild the best score panel
			foreach(Transform t in content)
			{
				Destroy(t.gameObject);
			}

			for(int i = 0; i < playerData.GetEntries(); ++i)
			{
				AddScoreEntry(playerData.GetKey(i), playerData.GetValue(i), i % 2 != 0);
			}
		}
	}

	void AddScoreEntry(string _key, string _value, bool _transparentBG)
	{
		GameObject go = Instantiate(personalHighScorePrefab, content);
		TMP_Text key = go.transform.GetChild(0).GetComponent<TMP_Text>();
		key.text = _key;
		TMP_Text value = go.transform.GetChild(1).GetComponent<TMP_Text>();
		value.text = _value;

		if(_transparentBG)
		{
			Image image = go.GetComponent<Image>();
			image.color = new Color(0, 0, 0, 0);
		}
	}

}
