using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
	public int CurrentHealth { get; }
	public void DoDestroy(PlayerMechanics causedBy = null);
	public void DoDamage(int damage, PlayerMechanics causedBy = null);
}
