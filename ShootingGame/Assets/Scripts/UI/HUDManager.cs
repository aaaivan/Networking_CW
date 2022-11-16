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
	string enemiesTextVar = "Enemies Left: {0}";
	[SerializeField]
	string livesTextVar = "Lives: {0}";
	int enemiesCount = 0;
	Transform hud;
	Transform endGameScreen;
	[SerializeField]
	TMP_Text endGameMessage = null;

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerDead += UpdateEnemiesCounter;
		PlayerMechanics.OnPlayerKilled += UpdateLivesCounter;
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerDead -= UpdateEnemiesCounter;
		PlayerMechanics.OnPlayerKilled -= UpdateLivesCounter;
	}

	private void Awake()
	{
		enemiesText.text = string.Format(enemiesTextVar, enemiesCount);
		livesText.text = string.Format(livesTextVar, localPlayer.LivesLeft);
		hud = transform.GetChild(0);
		endGameScreen = transform.GetChild(1);
		hud.gameObject.SetActive(true);
		endGameScreen.gameObject.SetActive(false);
	}

	public void NumberOfEnemiesSet(int n)
	{
		enemiesCount = n;
		enemiesText.text = string.Format(enemiesTextVar, enemiesCount);
	}

	private void UpdateEnemiesCounter(PlayerMechanics player)
	{
		if(player.IsHuman)
		{
			EndGame(false);
		}
		else
		{
			--enemiesCount;
			enemiesText.text = string.Format(enemiesTextVar, enemiesCount);
			if (enemiesCount == 0)
			{
				EndGame(true);
			}
		}
	}

	private void UpdateLivesCounter(PlayerMechanics player)
	{
		if (player == localPlayer)
		{
			livesText.text = string.Format(livesTextVar, localPlayer.LivesLeft);
		}
	}

	private void EndGame(bool hasWon)
	{
		hud.gameObject.SetActive(false);
		endGameScreen.gameObject.SetActive(true);

		ThirdPersonController fpc = InputsManager.Instance.thirdPersonController;
		GameObject player = fpc.gameObject;
		StarterAssetsInputs inputs = player.GetComponent<StarterAssetsInputs>();
		if (Input.mousePresent)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			inputs.cursorLocked = false;
			inputs.cursorInputForLook = false;
		}
		fpc.DisableGameInputs();

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
