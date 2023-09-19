using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProjectileType { Raycast, Projectile };
	
public enum EFireMode { Single, Burst, Auto };

public class BaseGun : BaseWeapon
{
	public EProjectileType ProjectileTypeEnum = EProjectileType.Raycast;
	public EFireMode FireModeEnum = EFireMode.Single;
		
	private EFireMode _previousFireModeEnum;
	private EProjectileType _previousProjectileTypeEnum;
	
	protected IProjectileType _projectileType;
	protected IFiringMode _firingMode;

	
	private void Awake()
	{
		InitializeFiringMode();
	}
	
	private void OnValidate()
	{
		if (ProjectileTypeEnum != _previousProjectileTypeEnum)
		{
			InitializeProjectileType();
			_previousProjectileTypeEnum = ProjectileTypeEnum;
		}
		
		if (FireModeEnum != _previousFireModeEnum)
		{
			InitializeFiringMode();
			_previousFireModeEnum = FireModeEnum;
		}
	}

	public override void Attack()
	{
		Shoot();
	}

	public virtual void Shoot()
	{
		if (_firingMode == null)
		{
			Debug.LogError("Firing mode is not initialized.");
			return;
		}
		
		if(_projectileType == null)
		{
			Debug.LogError("Projectile type is not initialized.");
			return;
		}
		
		_firingMode.ExecuteFiringSequence(WeaponData, _projectileType);
	}

	public void SwitchFireMode(EFireMode newFireMode)
	{
		FireModeEnum = newFireMode;
		InitializeFiringMode();
	}
	
	#region Util Methods
	
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
		switch (ProjectileTypeEnum)
		{
			case EProjectileType.Projectile:
				_projectileType = AddOrGetComponent<PhysicalProjectile>();
			break;
			
			case EProjectileType.Raycast:
				_projectileType = AddOrGetComponent<RaycastProjectile>();
			break;
			
			default:
				Debug.LogError("Unsupported ProjectileType: " + ProjectileTypeEnum);
			break;
		}
	}

	private void GetProjectileType()
	{
		if(_projectileType != null || ProjectileTypeEnum != _previousProjectileTypeEnum)
		{
			// Initialize ProjectileType based on the enum
			switch (ProjectileTypeEnum)
			{
			case EProjectileType.Projectile:
				_projectileType = GetComponent<PhysicalProjectile>();
				break;
				
			case EProjectileType.Raycast:
				_projectileType = GetComponent<RaycastProjectile>();
				break;
				
			default:
				Debug.LogError("Unsupported ProjectileType: " + ProjectileTypeEnum);
				break;
			}
			
			_previousProjectileTypeEnum = ProjectileTypeEnum;
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
		switch (FireModeEnum)
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
			
			default:
				Debug.LogError("Unsupported FireMode: " + FireModeEnum);
			break;
		}
	}
	
	private void GetFiringMode()
	{
		if(_firingMode == null || FireModeEnum != _previousFireModeEnum)
		{
			// Initialize FiringMode based on the enum
			switch (FireModeEnum)
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
				
			default:
				Debug.LogError("Unsupported FireMode: " + FireModeEnum);
				break;
			}
			
			_previousFireModeEnum = FireModeEnum;
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
	
	#endregion
}
