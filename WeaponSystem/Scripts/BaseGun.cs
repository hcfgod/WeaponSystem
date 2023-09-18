using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFireMode { Single, Burst, Auto };
public enum EProjectileType { Raycast, Projectile };
	
public class BaseGun : BaseWeapon
{

	public EFireMode FireMode = EFireMode.Single;
	public EProjectileType ProjectileType = EProjectileType.Projectile;
	
	public override void Attack()
	{
		Shoot();
	}
	
	public virtual void Shoot()
	{
		
	}
}
