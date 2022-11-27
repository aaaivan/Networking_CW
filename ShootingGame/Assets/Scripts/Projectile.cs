using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Projectile : MonoBehaviour, IDestructible
{
	[SerializeField]
	int damage = 1;
	[SerializeField]
	int currentHealth = 2;
	public int CurrentHealth { get { return currentHealth; } }
	bool isColliding = false;

	public PlayerMechanics shotBy { get; set; }


	public void DoDestroy(PlayerMechanics causedBy = null)
	{
		currentHealth = 0;
		Destroy(gameObject);
		AudioManager.Instance.Play3dSound("ProjectileDestruction", gameObject.transform.position);
	}

	public void DoDamage(int damage, PlayerMechanics causedBy = null)
	{
		if (currentHealth <= 0)
			return;

		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			DoDestroy();
		}
		else
		{
			AudioManager.Instance.Play3dSound("ProjectileImpact", gameObject.transform.position);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		isColliding = true;
	}

	private void OnCollisionExit(Collision collision)
    {
		// the function is called once for each collider on the hit object
		// use this flag to exit the funtion earlier if this is the case.
		if (!isColliding)
			return;

		isColliding = false;

		IDestructible gameObj = collision.gameObject.GetComponent<IDestructible>();
		if (gameObj != null)
        {
			// the hit object is of the Distructible type
			PlayerMechanics player = collision.gameObject.GetComponent<PlayerMechanics>();
			// If the hit object is a player, make sure it is not a remote player.
			// They will handle damage to themselves.
			if (player == null || !player.IsRemotePlayer)
			{
				gameObj.DoDamage(damage, shotBy);
			}
			DoDamage(currentHealth);
        }
		else if(currentHealth > 0)
		{
			DoDamage(1);
		}
    }
}