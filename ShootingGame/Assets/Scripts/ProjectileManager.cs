using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour, Destructable
{
	[SerializeField]
	int damage = 1;
	[SerializeField]
	int health = 1;
	public int Health { get { return health; } }
	public Vector3 RespownPosition { get { return Vector3.zero; } set { ; } }


	public void DoDestroy()
	{
		health = 0;
		Destroy(gameObject);
		AudioManager.Instance.Play3dSound("ProjectileDestruction", gameObject.transform.position);
	}

	public void DoDamage(int damage = 1)
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
		Destructable gameObj = collision.gameObject.GetComponent<Destructable>();
		if (gameObj != null)
        {
			gameObj.DoDamage(damage);
			DoDamage(health);
        }
		else if(health > 0)
		{
			DoDamage();
		}
    }
}