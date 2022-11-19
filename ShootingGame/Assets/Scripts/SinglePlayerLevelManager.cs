using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerLevelManager : MonoBehaviour
{
	[SerializeField]
	int gameTime = 60;
	float startTime = 0.0f;
	public int TimeLeft { get { return (int)(gameTime - Time.time + startTime); } }
	bool isPlaying = false;

	int enemiesCount = 0;
	public int EnemiesCount { get { return enemiesCount; } }

	[SerializeField]
	HUDManager hud = null;

	static SinglePlayerLevelManager instance;
	static public SinglePlayerLevelManager Instance { get { return instance; } }

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerDead += UpdateEnemiesCounter;
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerDead -= UpdateEnemiesCounter;
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
		if(instance == null)
		{
			instance = this;
			isPlaying = true;
			startTime = Time.time;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		enemiesCount = EnemiesSpawner.Instance.EnemiesNumber;
	}

	private void Update()
	{
		if(isPlaying && TimeLeft <= 0)
		{
			EndGame(false);
		}
	}

	private void UpdateEnemiesCounter(PlayerMechanics player)
	{
		if (player.IsLocalPlayer)
		{
			EndGame(false);
		}
		else
		{
			--enemiesCount;
			if (enemiesCount == 0)
			{
				EndGame(true);
			}
		}
	}

	private void EndGame(bool hasWon)
	{
		InputsManager.Instance.DisableThirdPersonInputs();
		isPlaying = false;
		hud.ShowEndGame(hasWon);
	}
}
