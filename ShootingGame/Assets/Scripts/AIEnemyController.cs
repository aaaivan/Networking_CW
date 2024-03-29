using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class AIEnemyController : MonoBehaviour
{
	[SerializeField]
	float playerDetectionDistance = 10;
	[SerializeField]
	float ShootingTimeout = 1.0f;

	private PlayerMechanics mechanics = null;
	private Animator animator = null;
	private NavMeshAgent agent = null;
	private GameObject[] players = null;
	private GameObject chasedPlayer = null;

	private float lastShotTime;
	private bool canShoot = false;

	private int _animIDSpeed;
	private int _animIDAttack;
	private int _animIDMotionSpeed;

	private void Awake()
	{
		mechanics = GetComponent<PlayerMechanics>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		players = GameObject.FindGameObjectsWithTag("Player");
		AssignAnimationIDs();
	}

	private void AssignAnimationIDs()
	{
		_animIDSpeed = Animator.StringToHash("Speed");
		_animIDAttack = Animator.StringToHash("Attack");
		_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
	}

	void Update()
    {
        if (chasedPlayer == null)
		{
			foreach (var p in players)
			{
				Vector3 relPosition = p.transform.position - transform.position;
				// is the player is closer than the detection treshhold start chasing
				if (relPosition.magnitude < playerDetectionDistance)
				{
					chasedPlayer = p;
					agent.updateRotation = false;
					StartCoroutine(ChaseCoroutine());
					break;
				}
			}
		}
		else
		{
			Vector3 relPosition = chasedPlayer.transform.position - transform.position;
			// if the chased player is further than the detection distance, stop chasing
			if (relPosition.magnitude > playerDetectionDistance)
			{
				chasedPlayer = null;
				agent.updateRotation = true;
				canShoot = false;
				StopCoroutine(ChaseCoroutine());
			}
			else
			{
				RotateTowards(relPosition);
				Shoot();
			}
		}

		animator.SetFloat(_animIDSpeed, agent.velocity.magnitude);
		animator.SetFloat(_animIDMotionSpeed, agent.velocity.magnitude/agent.speed);
    }

	IEnumerator ChaseCoroutine()
	{
		// wait 2 seconds before chasing
		yield return new WaitForSeconds(2);
		canShoot = true;
		while (chasedPlayer != null)
		{
			Chase();
			// recalculate the path every 2 seconds
			yield return new WaitForSeconds(2);
		}
		yield return null;
	}

	/// <summary>
	/// set the new destination for the enemy navmesh.
	/// the target destination is right in front of the player character
	/// </summary>
	private void Chase()
	{
		Vector3 chaseDir = chasedPlayer.transform.position - transform.position;
		chaseDir.Normalize();
		agent.SetDestination(chasedPlayer.transform.position - 5 * chaseDir);
	}

	private void Shoot()
	{
		if (canShoot && Time.time > ShootingTimeout + lastShotTime)
		{
			lastShotTime = Time.time;
			animator.SetTrigger(_animIDAttack);
		}
	}

	private void OnShoot()
	{
		mechanics.Shoot();
	}

	private void RotateTowards(Vector3 direction)
	{
		// we do not want to have a pitch component to the rotation
		direction.y = 0;
		// create the rotation we need to be in to look at the target
		Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
		
		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Mathf.PI/180 * Time.deltaTime * agent.angularSpeed);
	}
}
