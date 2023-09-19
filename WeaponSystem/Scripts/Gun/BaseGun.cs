using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum EProjectileType { Raycast, Projectile };
	
public enum EFireMode { Single, Burst, Auto };

public enum EAmmoBehavior { Realistic, CallOfDuty };

public class BaseGun : BaseWeapon
{
	#region Events
	
	public UnityEvent2 OnWeaponFireModeSwitchedUnityEvent;	
	public delegate void WeaponFireModeSwitched();
	public event WeaponFireModeSwitched OnWeaponFireModeSwitched;
	
	#endregion
	
	public GunData GunDataRef;
	public AmmoBehaviorData AmmoBehaviorDataRef;
	
	public EProjectileType ProjectileTypeEnum = EProjectileType.Raycast;
	public EFireMode FireModeEnum = EFireMode.Single;
	public EAmmoBehavior AmmoBehaviorEnum = EAmmoBehavior.Realistic;
		
	private EFireMode _previousFireModeEnum;
	private EProjectileType _previousProjectileTypeEnum;
	private EAmmoBehavior _previousAmmoBehaviorEnum;
	
	protected IProjectileType _projectileType;
	protected IFiringMode _firingMode;
	protected IAmmoBehavior _ammoBehavior;
	public IAmmoBehavior AmmoBehavior { get { return _ammoBehavior; } }
	
	private void Awake()
	{
		InitializeFiringMode();
		InitializeProjectileType();
		InitializeAmmoBehaviour();
	}
	
	private void OnValidate()
	{	
		if (FireModeEnum != _previousFireModeEnum)
		{
			InitializeFiringMode();
			_previousFireModeEnum = FireModeEnum;
		}
		
		if (ProjectileTypeEnum != _previousProjectileTypeEnum)
		{
			InitializeProjectileType();
			_previousProjectileTypeEnum = ProjectileTypeEnum;
		}
		
		if (AmmoBehaviorEnum != _previousAmmoBehaviorEnum)
		{
			InitializeAmmoBehaviour();
			_previousAmmoBehaviorEnum = AmmoBehaviorEnum;
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
		
		if (!_ammoBehavior.CanShoot())
		{
			return;
		}
		
		_firingMode.ExecuteFiringSequence(WeaponDataRef, GunDataRef, _projectileType);
	}

	public void SwitchFireMode(EFireMode newFireMode)
	{
		FireModeEnum = newFireMode;
		InitializeFiringMode();
		
		OnWeaponFireModeSwitched?.DynamicInvoke();
		OnWeaponFireModeSwitchedUnityEvent?.Invoke();
	}
	
	#region Util Methods
	
	private void InitializeProjectileType()
	{				
		System.Type expectedType = GetProjectileTypeFromEnum();

		// Remove components that don't match the enum
		foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
		{
			if (component is IProjectileType && component.GetType() != expectedType)
			{
            #if UNITY_EDITOR
				DestroyImmediate(component);
            #else
				Destroy(component);
            #endif
			}
		}

		// Initialize ProjectileType based on the enum
		if (expectedType == typeof(RaycastProjectile))
		{
			_projectileType = ComponentUtils.AddOrGetComponent<RaycastProjectile>(gameObject);
		}
		else if (expectedType == typeof(PhysicalProjectile))
		{
			_projectileType = ComponentUtils.AddOrGetComponent<PhysicalProjectile>(gameObject);
		}
		else
		{
			Debug.LogError("Unsupported ProjectileType: " + ProjectileTypeEnum);
		}

		ComponentUtils.RemoveDuplicateComponents<IProjectileType>(gameObject);
	}
	
	private void InitializeFiringMode()
	{
		System.Type expectedType = GetFiringModeTypeFromEnum();

		// Remove components that don't match the enum
		foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
		{
			if (component is IFiringMode && component.GetType() != expectedType)
			{
	            #if UNITY_EDITOR
					DestroyImmediate(component);
	            #else
					Destroy(component);
	            #endif
			}
		}

		// Initialize FiringMode based on the enum
		if (expectedType == typeof(SingleShot))
		{
			_firingMode = ComponentUtils.AddOrGetComponent<SingleShot>(gameObject);
		}
		else if (expectedType == typeof(FullAuto))
		 {
			_firingMode = ComponentUtils.AddOrGetComponent<FullAuto>(gameObject);
		 }
		// Uncomment these lines when you have the corresponding classes
		// else if (expectedType == typeof(Burst))
		// {
		//     _firingMode = AddOrGetComponent<Burst>();
		// }
		else
		{
			Debug.LogError("Unsupported FireMode: " + FireModeEnum);
		}

		ComponentUtils.RemoveDuplicateComponents<IFiringMode>(gameObject);
	}
	
	private void InitializeAmmoBehaviour()
	{
		System.Type expectedType = GetAmmoBehaviourFromEnum();

		// Remove components that don't match the enum
		foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
		{
			if (component is IAmmoBehavior && component.GetType() != expectedType)
			{
	            #if UNITY_EDITOR
				DestroyImmediate(component);
	            #else
				Destroy(component);
	            #endif
			}
		}

		// Initialize FiringMode based on the enum
		if (expectedType == typeof(RealisticAmmoBehavior))
		{
			_ammoBehavior = ComponentUtils.AddOrGetComponent<RealisticAmmoBehavior>(gameObject);
		}
		// Uncomment these lines when you have the corresponding classes
		// else if (expectedType == typeof(CallOfDuty))
		// {
		//     _firingMode = AddOrGetComponent<Burst>();
		// }
		else
		{
			Debug.LogError("Unsupported Ammo Behaviour: " + AmmoBehaviorEnum);
		}

		ComponentUtils.RemoveDuplicateComponents<IFiringMode>(gameObject);
	}
	
	private Type GetFiringModeTypeFromEnum()
	{
		switch (FireModeEnum)
		{
		case EFireMode.Auto:
			return typeof(FullAuto);
		case EFireMode.Single:
			return typeof(SingleShot);
		case EFireMode.Burst:
			//return typeof(Burst);
		default:
			return null;
		}
	}

	private Type GetProjectileTypeFromEnum()
	{
		switch (ProjectileTypeEnum)
		{
		case EProjectileType.Raycast:
			return typeof(RaycastProjectile);
		case EProjectileType.Projectile:
			return typeof(PhysicalProjectile);
		default:
			return null;
		}
	}
	
	private Type GetAmmoBehaviourFromEnum()
	{
		switch (AmmoBehaviorEnum)
		{
			case EAmmoBehavior.Realistic:
				return typeof(RealisticAmmoBehavior);
			
			case EAmmoBehavior.CallOfDuty:
				//return typeof(RealisticAmmo);
				
			default:
				return null;
		}
	}
	
	#endregion
}
