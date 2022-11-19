using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Destructable gameObj = collision.gameObject.GetComponent<Destructable>();
		if (gameObj != null)
		{
			PlayerMechanics player = collision.gameObject.GetComponent<PlayerMechanics>();
			if(player == null || !player.IsRemotePlayer)
			{
				gameObj.DoDestroy();
			}
		}
	}
}
