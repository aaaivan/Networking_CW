using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnManager : MonoBehaviour
{
	public Transform respawnPosition;
	static RespawnManager instance;
	static public RespawnManager Instance
	{
		get
		{
			if (instance == null)
				throw new UnityException("You need to add a RespawnManager to your scene");
			return instance;
		}
	}

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerKilled += ctx => Respawn(ctx);
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerKilled -= ctx => Respawn(ctx);
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}
	}
	
	public void Respawn(PlayerMechanics player)
	{
		if(player.IsHuman)
		{
			player.transform.position = respawnPosition.position;
			player.transform.rotation = respawnPosition.rotation;
			ThirdPersonController tpc = player.gameObject.GetComponent<ThirdPersonController>();
			if (tpc != null)
			{
				tpc.Reset();
			}
			Physics.SyncTransforms();
		}
		else
		{
			NavMeshAgent agent = player.gameObject.GetComponent<NavMeshAgent>();
			player.transform.rotation = respawnPosition.rotation;
			agent.Warp(respawnPosition.position);
		}
		player.RestoreHealth();
	}
}
