using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		IDestructible gameObj = collision.gameObject.GetComponent<IDestructible>();
		if (gameObj != null)
		{
			// the object is of type Destructible
			PlayerMechanics player = collision.gameObject.GetComponent<PlayerMechanics>();
			
			// check if the object is a player, if yes make sure it is not a remote player
			// as they will handle their destruction themselves
			if(player == null || !player.IsRemotePlayer)
			{
				gameObj.DoDestroy();
			}
		}
	}
}
