using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFireMode { Single, Burst, Auto };
public enum EProjectileType { Raycast, Projectile };
	
public class BaseGun : BaseWeapon
{
	public EFireMode FireMode = EFireMode.Single;
	public EProjectileType ProjectileType = EProjectileType.Projectile;

	private EFireMode previousFireMode;
	private EProjectileType previousProjectileType;
	
	protected IProjectileType _projectileType;
	protected IFiringMode _firingMode;

	private void OnValidate()
	{
		if (FireMode != previousFireMode)
		{
			InitializeFiringMode();
			previousFireMode = FireMode;
		}

		if (ProjectileType != previousProjectileType)
		{
			InitializeProjectileType();
			previousProjectileType = ProjectileType;
		}
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
		
		if (_projectileType != null)
		{
			DestroyImmediate(_projectileType as MonoBehaviour);
		}
		
		#endif
		
	    #if !UNITY_EDITOR

		if (_projectileType != null)
		{
			Destroy(_projectileType as MonoBehaviour);
		}
		
		#endif
		
		// Initialize ProjectileType based on the enum
		switch (ProjectileType)
		{
			case EProjectileType.Projectile:
				_projectileType = AddOrGetComponent<PhysicalProjectile>();
			break;
			
			case EProjectileType.Raycast:
				_projectileType = AddOrGetComponent<RaycastProjectile>();
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
		if (_firingMode != null)
		{
		Destroy(_firingMode as MonoBehaviour);
		}
		
		#endif

		// Initialize FiringMode based on the enum
		switch (FireMode)
		{
			case EFireMode.Auto:
				//_firingMode = AddOrGetComponent<Automatic>();
			break;
				
			case EFireMode.Single:
				_firingMode = AddOrGetComponent<SingleShot>();
			break;
				
			case EFireMode.Burst:
				//_firingMode = AddOrGetComponent<Burst>();
			break;
		}
	}
	
	private T AddOrGetComponent<T>() where T : MonoBehaviour, new()
	{
		T component = GetComponent<T>();
    
		if (component == null)
		{
			component = gameObject.AddComponent<T>();
		}
    
		return component;
	}
}
