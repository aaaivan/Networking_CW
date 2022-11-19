using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, Destructable
{
	[SerializeField]
	int damage = 1;
	[SerializeField]
	int health = 1;
	public int Health { get { return health; } }
	public Vector3 RespownPosition { get { return Vector3.zero; } set { ; } }
	bool isBeingDestroyed = false;

	public PlayerMechanics shotBy { get; set; }


	public void DoDestroy(PlayerMechanics causedBy = null)
	{
		health = 0;
		Destroy(gameObject);
		AudioManager.Instance.Play3dSound("ProjectileDestruction", gameObject.transform.position);
	}

	public void DoDamage(int damage, PlayerMechanics causedBy = null)
	{
		if (health <= 0)
			return;

		health -= damage;
		if (health <= 0)
		{
			DoDestroy();
		}
		else
		{
			AudioManager.Instance.Play3dSound("ProjectileImpact", gameObject.transform.position);
		}
	}
	private void OnCollisionExit(Collision collision)
    {
		if (isBeingDestroyed)
			return;

		Destructable gameObj = collision.gameObject.GetComponent<Destructable>();
		if (gameObj != null)
        {
			PlayerMechanics player = collision.gameObject.GetComponent<PlayerMechanics>();
			if (player == null || !player.IsRemotePlayer)
			{
				gameObj.DoDamage(damage, shotBy);
			}
			DoDamage(health);
			isBeingDestroyed = true;
        }
		else if(health > 0)
		{
			DoDamage(1);
		}
    }
}