using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject enemyPrefab;
	[SerializeField]
	Transform[] spawnPositions;
	HUDManager hudManager;

	private void Awake()
	{
		hudManager = GameObject.FindGameObjectsWithTag("HUD")[0].GetComponent<HUDManager>();
		foreach(Transform t in spawnPositions)
		{
			Instantiate(enemyPrefab, t);
		}
		hudManager.NumberOfEnemiesSet(spawnPositions.Length);
	}
}
