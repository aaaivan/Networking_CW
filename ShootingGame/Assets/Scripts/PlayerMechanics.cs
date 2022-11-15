using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class PlayerMechanics : MonoBehaviour, Destructable
{
	[Header("Health and Lives")]
	[SerializeField]
	int lives = 3;
	int deathCount = 0;
	[SerializeField]
	int health = 3;
	int currentHelath = 0;
	[SerializeField]
	HealthBar healthBar = null;

	public int Health { get { return health; } }
	Vector3 respownTransform = Vector3.zero;
	public Vector3 RespownPosition { get { return respownTransform; } set { respownTransform = value; } }

	[Header("Shooting")]
	[SerializeField] GameObject projectilePrefab = null;
	Collider[] playerColliders;
	[SerializeField] Transform spawnPosition = null;
	[SerializeField] float fireAngle = 45;
	[SerializeField] float firePower = 2;

	bool isHuman = true;
	public bool IsHuman { get { return isHuman; } }

	bool isLocalPlayer = false;
	public bool IsLocalPlayer { get { return isLocalPlayer; } }

	public delegate void PlayerKilled(PlayerMechanics p);
	public static event PlayerKilled OnPlayerKilled;

	public delegate void PlayerDead(PlayerMechanics p);
	public static event PlayerDead OnPlayerDead;

	private void Awake()
	{
		isHuman = (GetComponent<NavMeshAgent>() == null);
		PhotonView photonView = GetComponent<PhotonView>();
		isLocalPlayer = isHuman && (photonView == null || photonView.IsMine);

		playerColliders = GetComponents<Collider>();
		RespownPosition = transform.position;
		RestoreHealth();
	}

	public void DoDestroy()
	{
		if (isHuman && !isLocalPlayer)
			return;

		deathCount++;
		if(deathCount < lives && OnPlayerKilled != null)
		{
			if (OnPlayerKilled != null)
			{
				OnPlayerKilled.Invoke(this);
			}
		}
		else if (deathCount >= lives && OnPlayerKilled != null)
		{
			if (OnPlayerDead != null)
			{
				OnPlayerDead.Invoke(this);
			}

			if (isHuman)
			{
				gameObject.SetActive(false);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
	public void DoDamage(int damage = 1)
	{
		if (isHuman && !isLocalPlayer)
			return;

		Debug.Log("damage");
		currentHelath -= damage;
		healthBar.SetFillAmount((float)currentHelath/health);
		if(currentHelath <= 0)
		{
			DoDestroy();
		}
	}

	public void RestoreHealth()
	{
		if (isHuman && !isLocalPlayer)
			return;

		currentHelath = health;
		healthBar.SetFillAmount(1.0f);
	}

	public void Shoot()
	{
		if (isHuman && !isLocalPlayer)
			return;

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
