using UnityEngine;

public class RangedWeapon : BaseGun
{
	private IProjectileType _projectileType;
	private IFiringMode _firingMode;

	private void Awake()
	{
		_projectileType = GetComponent<IProjectileType>();
		
		switch(ProjectileType)
		{
			case EProjectileType.Projectile:
				_projectileType = GetComponent<PhysicalProjectile>();
			break;
			
			case EProjectileType.Raycast:
				_projectileType = GetComponent<RaycastProjectile>();
			break;
		}
		
		switch(FireMode)
		{
			case EFireMode.Auto:
				_firingMode = GetComponent<SingleShot>();
			break;
				
			case EFireMode.Single:
				_firingMode = GetComponent<SingleShot>();
			break;
				
			case EFireMode.Burst:
				_firingMode = GetComponent<SingleShot>();
			break;
		}
	}

	public override void Shoot()
	{
		_firingMode.ExecuteFiringSequence(_projectileType);
	}
}