using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
	[SerializeField]
	TMP_Text textAsset = null;
	[SerializeField]
	string textVar = "Enemies Left: {0}";
	int enemiesCount = 5;

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerDead += ctx => UpdateHUDText();
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerDead -= ctx => UpdateHUDText();
	}
	private void Awake()
	{
		textAsset.text = string.Format(textVar, enemiesCount);
	}

	private void UpdateHUDText()
	{
		--enemiesCount;
		textAsset.text = string.Format(textVar, enemiesCount);
	}
}
