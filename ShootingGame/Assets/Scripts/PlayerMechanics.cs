using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMechanics : MonoBehaviour, Destructable, IPunObservable
{
	[Header("Health and Lives")]
	[SerializeField]
	int lives = 3;
	int deathCount = 0;
	[SerializeField]
	int health = 3;
	int currentHealth = 0;

	public int Health { get { return health; } }
	public int CurrentHealth { get { return currentHealth; } }
	public int LivesLeft { get { return lives - deathCount; } }
	public int Deaths { get { return deathCount; } }
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

	Photon.Realtime.Player owner = null;

	public delegate void PlayerKilled(PlayerMechanics p);
	public static event PlayerKilled OnPlayerKilled;

	public delegate void PlayerDead(PlayerMechanics p);
	public static event PlayerDead OnPlayerDead;

	private void Awake()
	{
		isHuman = GetComponent<PlayerInput>() != null;
		PhotonView photonView = GetComponent<PhotonView>();
		isLocalPlayer = isHuman && (photonView == null || photonView.IsMine);
		if (photonView != null)
		{
			owner = photonView.Owner;
			MultiplayerLevelManager.Instance.RegisterPlayer(owner, this);
		}

		playerColliders = GetComponents<Collider>();
		RespownPosition = transform.position;
		RestoreHealth();
	}

	public void DoDestroy(PlayerMechanics causedBy = null)
	{
		if (isHuman && !isLocalPlayer)
			return;

		// Update score of the player who caused the damage
		if(causedBy != null && causedBy.owner != null)
		{
			causedBy.owner.AddScore(1);
		}

		deathCount++;
		TriggerDeathEvents();
	}

	private void TriggerDeathEvents()
	{
		if (deathCount < lives && OnPlayerKilled != null)
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

	public void DoDamage(int damage, PlayerMechanics causedBy = null)
	{
		if (isHuman && !isLocalPlayer)
			return;

		Debug.Log("damage");
		currentHealth -= damage;
		if(currentHealth <= 0)
		{
			DoDestroy(causedBy);
		}
	}

	public void RestoreHealth()
	{
		if (isHuman && !isLocalPlayer)
			return;

		currentHealth = health;
	}

	public void Shoot()
	{
		GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, spawnPosition.rotation);

		// Prevent the projectile to collide with th eplayer who shot it
		Collider projectileCollider = projectile.GetComponent<Collider>();
		foreach (var collider in playerColliders)
		{
			Physics.IgnoreCollision(projectileCollider, collider, true);
		}
		Rigidbody rigidBody = projectile.GetComponent<Rigidbody>();

		// Set owner of the projectile
		ProjectileManager projecitleManager = projectile.GetComponent<ProjectileManager>();
		projecitleManager.shotBy = this;

		// Apply impulse to the projectile
		Vector3 forceDirection = transform.forward * Mathf.Cos(Mathf.Deg2Rad * fireAngle) + transform.up * Mathf.Sin(Mathf.Deg2Rad * fireAngle);
		rigidBody.AddForce(forceDirection * firePower, ForceMode.Impulse);

		// Play sound
		AudioManager.Instance.Play3dSound("Shoot", gameObject.transform.position);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(currentHealth);
			stream.SendNext(deathCount);
		}
		else
		{
			currentHealth = (int)stream.ReceiveNext();
			int newDeathCount = (int)stream.ReceiveNext();
			if(newDeathCount > deathCount)
			{
				deathCount = newDeathCount;
				TriggerDeathEvents();
			}
		}
	}
}
