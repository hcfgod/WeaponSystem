using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : BaseWeapon
{
	public IFiringMode FiringMode { get; set; }

	public override void Attack()
	{
		FiringMode?.ExecuteFiringSequence();
	}
}
