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
	TMP_Text hudTextAsset = null;
	[SerializeField]
	string textVar = "Enemies Left: {0}";
	int enemiesCount = 0;
	Transform hud;
	Transform endGameScreen;
	[SerializeField]
	TMP_Text endGameMessage = null;

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerDead += UpdateEnemiesCounter;
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerDead -= UpdateEnemiesCounter;
	}

	private void Awake()
	{
		hudTextAsset.text = string.Format(textVar, enemiesCount);
		hud = transform.GetChild(0);
		endGameScreen = transform.GetChild(1);
		hud.gameObject.SetActive(true);
		endGameScreen.gameObject.SetActive(false);
	}

	private void UpdateHUDText()
	{
		hudTextAsset.text = string.Format(textVar, enemiesCount);
	}

	public void NumberOfEnemiesSet(int n)
	{
		enemiesCount = n;
		UpdateHUDText();
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
			UpdateHUDText();
			if (enemiesCount == 0)
			{
				EndGame(true);
			}
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
