using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnManager : MonoBehaviour
{
	public Transform respawnPosition;

	private void OnEnable()
	{
		PlayerMechanics.OnPlayerKilled += Respawn;
	}

	private void OnDisable()
	{
		PlayerMechanics.OnPlayerKilled -= Respawn;
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
