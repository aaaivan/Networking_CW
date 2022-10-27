using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour, Destructable
{
	[SerializeField]
	int lives = 3;
	int deathCount = 0;
	[SerializeField]
	int health = 1;
	public int Health { get { return health; } }
	public void DoDestroy()
	{
		deathCount++;
		if (deathCount < lives)
		{
			RespawnManager.Instance.Respawn(gameObject);
			ThirdPersonController tpc = GetComponent<ThirdPersonController>();
			if(tpc != null)
			{
				tpc.Reset();
			}
			Debug.Log("lives left" + (lives - deathCount));
		}
		else
		{
			Debug.Log("Game Over");
		}
	}
	public void DoDamage(int damage = 1)
	{
		health -= damage;
		if(health <= 0)
		{
			DoDestroy();
		}
	}
}
