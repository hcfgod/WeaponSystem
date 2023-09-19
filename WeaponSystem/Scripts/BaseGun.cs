using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFireMode { Single, Burst, Auto };
public enum EProjectileType { Raycast, Projectile };
	
public class BaseGun : BaseWeapon
{
	public EFireMode FireMode = EFireMode.Single;
	public EProjectileType ProjectileType = EProjectileType.Projectile;

	protected IProjectileType _projectileType;
	protected IFiringMode _firingMode;

	private void Awake()
	{
		InitializeProjectileType();
		InitializeFiringMode();
	}
	
	private void OnValidate()
	{
		InitializeProjectileType();
		InitializeFiringMode();
	}

	public override void Attack()
	{
		Shoot();
	}

	public virtual void Shoot()
	{
		_firingMode.ExecuteFiringSequence(WeaponData, _projectileType);
	}

	public void SwitchFireMode(EFireMode newFireMode)
	{
		FireMode = newFireMode;
		InitializeFiringMode();
	}

	private void InitializeProjectileType()
	{
		#if UNITY_EDITOR
		
		if (_firingMode != null)
		{
			DestroyImmediate(_projectileType as MonoBehaviour);
		}
		
		#endif
		
		#if !UNITY_EDITOR
		
		// Remove the old firing mode component if it exists
		if (_projectileType != null)
		{
			Destroy(_projectileType as MonoBehaviour);
		}
		
		#endif


		// Initialize ProjectileType based on the enum
		switch (ProjectileType)
		{
			case EProjectileType.Projectile:
				_projectileType = GetComponent<PhysicalProjectile>();
				
				if(_projectileType == null)
					_projectileType = gameObject.AddComponent<PhysicalProjectile>();
				break;
			case EProjectileType.Raycast:
				_projectileType = gameObject.AddComponent<RaycastProjectile>();
				
				if(_projectileType == null)
					_projectileType = gameObject.AddComponent<RaycastProjectile>();
				break;
		}
	}

	private void InitializeFiringMode()
	{
		#if UNITY_EDITOR
		
		if (_firingMode != null)
		{
			DestroyImmediate(_firingMode as MonoBehaviour);
		}
		
		#endif
		
	    #if !UNITY_EDITOR
		
		// Remove the old firing mode component if it exists
		if (_projectileType != null)
		{
			Destroy(_projectileType as MonoBehaviour);
		}
		
		#endif

		// Initialize FiringMode based on the enum
		switch (FireMode)
		{
		case EFireMode.Auto:
			//_firingMode = gameObject.AddComponent<Automatic>();
			break;
		case EFireMode.Single:
			_firingMode = gameObject.AddComponent<SingleShot>();
			break;
		case EFireMode.Burst:
			//_firingMode = gameObject.AddComponent<Burst>();
			break;
			// Add more cases as you implement more firing modes
		}
	}
}
