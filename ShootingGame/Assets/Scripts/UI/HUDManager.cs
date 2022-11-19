using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	PlayerMechanics localPlayer = null;
	[SerializeField]
	TMP_Text enemiesText = null;
	[SerializeField]
	TMP_Text livesText = null;
	[SerializeField]
	TMP_Text timerText = null;
	[SerializeField]
	string enemiesTextVar = "Enemies Left: {0}";
	[SerializeField]
	string livesTextVar = "Lives: {0}";
	int enemiesCount = 0;
	int livesCount = 0;
	int timer = 0;
	Transform hud;
	Transform endGameScreen;
	[SerializeField]
	TMP_Text endGameMessage = null;

	private void Awake()
	{
		hud = transform.GetChild(0);
		endGameScreen = transform.GetChild(1);
		hud.gameObject.SetActive(true);
		endGameScreen.gameObject.SetActive(false);
	}

	private void Start()
	{
		enemiesCount = SinglePlayerLevelManager.Instance.EnemiesCount;
		livesCount = localPlayer.LivesLeft;
		timer = SinglePlayerLevelManager.Instance.TimeLeft;
		enemiesText.text = string.Format(enemiesTextVar, enemiesCount);
		livesText.text = string.Format(livesTextVar, livesCount);
		timerText.text = timer.ToString();
	}

	private void LateUpdate()
	{
		if(enemiesCount != SinglePlayerLevelManager.Instance.EnemiesCount )
		{
			enemiesCount = SinglePlayerLevelManager.Instance.EnemiesCount;
			enemiesText.text = string.Format(enemiesTextVar, enemiesCount);
		}
		if(livesCount != localPlayer.LivesLeft)
		{
			livesCount = localPlayer.LivesLeft;
			livesText.text = string.Format(livesTextVar, livesCount);
		}
		if(timer != SinglePlayerLevelManager.Instance.TimeLeft)
		{
			timer = SinglePlayerLevelManager.Instance.TimeLeft;
			timerText.text = timer.ToString();
		}
	}

	public void ShowEndGame(bool hasWon)
	{
		hud.gameObject.SetActive(false);
		endGameScreen.gameObject.SetActive(true);

		if (hasWon)
		{
			endGameMessage.text = "You Won!";
		}
		else
		{
			endGameMessage.text = "You Lost!";
		}
	}
}
