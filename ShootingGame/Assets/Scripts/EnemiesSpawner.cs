using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject enemyPrefab;
	[SerializeField]
	Transform[] spawnPositions;
	[SerializeField]
	HUDManager hudManager;

	private void Awake()
	{
		foreach(Transform t in spawnPositions)
		{
			Instantiate(enemyPrefab, t);
		}
		hudManager.NumberOfEnemiesSet(spawnPositions.Length);
	}
}
