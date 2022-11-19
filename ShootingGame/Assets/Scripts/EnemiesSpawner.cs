using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
	[SerializeField]
	GameObject enemyPrefab;
	[SerializeField]
	Transform[] spawnPositions;
	public int EnemiesNumber { get { return spawnPositions.Length; } }

	static EnemiesSpawner instance;
	static public EnemiesSpawner Instance { get { return instance; } }

	private void OnDestroy()
	{
		if(instance == this)
		{
			instance = null;
		}
	}

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			foreach (Transform t in spawnPositions)
			{
				Instantiate(enemyPrefab, t);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
