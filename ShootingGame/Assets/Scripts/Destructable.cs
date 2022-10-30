using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Destructable
{
	public int Health { get; }
	public void DoDestroy();
	public void DoDamage(int damage = 1);
}
