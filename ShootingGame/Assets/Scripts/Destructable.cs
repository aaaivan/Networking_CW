using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Destructible
{
	public int Health { get; }
	public void DoDestroy(PlayerMechanics causedBy = null);
	public void DoDamage(int damage, PlayerMechanics causedBy = null);
	public Vector3 RespownPosition { get; set; }
}
