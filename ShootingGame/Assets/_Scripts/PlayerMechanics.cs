using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMechanics : MonoBehaviour, Destructable
{
	[Header("Health and Lives")]
	[SerializeField]
	int lives = 3;
	int deathCount = 0;
	[SerializeField]
	int health = 3;
	int currentHelath = 0;
	public int Health { get { return health; } }

	[Header("Shooting")]
	[SerializeField] GameObject projectilePrefab = null;
	Collider[] playerColliders;
	[SerializeField] Transform spawnPosition = null;
	[SerializeField] float fireAngle = 45;
	[SerializeField] float firePower = 2;

	bool isHuman = true;

	private void Awake()
	{
		isHuman = GetComponent<NavMeshAgent>() == null;
		playerColliders = GetComponents<Collider>();
		RestoreHealth();
	}

	public void DoDestroy()
	{
		deathCount++;
		if (deathCount < lives)
		{
			RespawnManager.Instance.Respawn(this);
			ThirdPersonController tpc = GetComponent<ThirdPersonController>();
			if(tpc != null)
			{
				tpc.Reset();
			}
		}
		else
		{
			Debug.Log("Game Over");
		}
	}
	public void DoDamage(int damage = 1)
	{
		currentHelath -= damage;
		if(currentHelath <= 0)
		{
			DoDestroy();
		}
	}

	public void RestoreHealth()
	{
		currentHelath = health;
	}

	public void Shoot()
	{
		GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation);
		Collider projectileCollider = projectile.GetComponent<Collider>();
		foreach (var collider in playerColliders)
		{
			Physics.IgnoreCollision(projectileCollider, collider, true);
		}
		Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();
		Vector3 forceDirection = transform.forward * Mathf.Cos(Mathf.Deg2Rad * fireAngle) + transform.up * Mathf.Sin(Mathf.Deg2Rad * fireAngle);
		rigidBody.AddForce(forceDirection * firePower, ForceMode.Impulse);

		// play sound
		AudioManager.Instance.Play3dSound("Shoot", gameObject.transform.position);
	}
}
