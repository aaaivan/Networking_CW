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

	/// <summary>
	/// Respawn player at their respawn location
	/// </summary>
	/// <param name="player"> player or enemy to respawn </param>
	public void Respawn(PlayerMechanics player)
	{
		if(player.IsRemotePlayer) // remote player will handle their respawn themselves
		{
			return;
		}

		Vector3 respawnPosition = player.RespownPosition;
		if(respawnPosition != null)
		{
			if (player.IsHuman) // the player is a human character
			{
				player.transform.position = respawnPosition;
				ThirdPersonController tpc = player.gameObject.GetComponent<ThirdPersonController>();
				if (tpc != null)
				{
					tpc.Reset();
				}
				Physics.SyncTransforms(); // needed to notify the Physics engine about the change in Transform;
				player.RestoreHealth();
			}
			else // the player is an AI controlled character
			{
				NavMeshAgent agent = player.gameObject.GetComponent<NavMeshAgent>();
				agent.Warp(respawnPosition);
				player.RestoreHealth();
			}
		}
	}
}
