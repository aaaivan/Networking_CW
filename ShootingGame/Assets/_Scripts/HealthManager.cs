using StarterAssets;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	GameObject player;
	int deathCount = 0;


	private void Awake()
	{
		//player = transform.parent.gameObject;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 4 /* water (used for the lava) */)
		{
			Respawn();
		}
	}

	private void Respawn()
	{
		RespawnManager.Instance.Respawn(gameObject);
		player.GetComponent<ThirdPersonController>().Reset();
		++deathCount;
	}
}
