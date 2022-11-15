using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnManager : MonoBehaviour
{
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
		Vector3 respawnPosition = player.RespownPosition;
		if(respawnPosition != null)
		{
			if (player.IsHuman)
			{
				if (player.IsLocalPlayer)
				{
					player.transform.position = respawnPosition;
					ThirdPersonController tpc = player.gameObject.GetComponent<ThirdPersonController>();
					if (tpc != null)
					{
						tpc.Reset();
					}
					Physics.SyncTransforms();
					player.RestoreHealth();
				}
			}
			else
			{
				NavMeshAgent agent = player.gameObject.GetComponent<NavMeshAgent>();
				agent.Warp(respawnPosition);
				player.RestoreHealth();
			}
		}
	}
}
