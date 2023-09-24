using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum EProjectileType { Raycast, Projectile };
	
public enum EFireMode { Single, Burst, Auto };

public enum EAimingMode { FirstPerson, ThirdPerson };

public enum EAmmoBehavior { Realistic, CallOfDuty };

public class BaseGun : BaseWeapon
{
	#region Events
	
	public UnityEvent2 OnWeaponFireModeSwitched;	
	public delegate void WeaponFireModeSwitched();
	public event WeaponFireModeSwitched OnWeaponFireModeSwitchedEvent;
	
	#endregion
	
	[Space(10)]
	
	public GunData GunDataRef;
	public AmmoBehaviorData AmmoBehaviorDataRef;
	
	public EProjectileType ProjectileTypeEnum = EProjectileType.Raycast;
	public EFireMode FireModeEnum = EFireMode.Single;
	public EAimingMode AimingModeEnum = EAimingMode.FirstPerson;
	public EAmmoBehavior AmmoBehaviorEnum = EAmmoBehavior.Realistic;
		
	private EFireMode _previousFireModeEnum;
	private EProjectileType _previousProjectileTypeEnum;
	private EAimingMode _previousAimingModeEnum;
	private EAmmoBehavior _previousAmmoBehaviorEnum;
	
	private IProjectileType _projectileType;
	private IFiringMode _firingMode;
	private IAimingMode _aimingMode;
	private IAmmoBehavior _ammoBehavior;
	public IAmmoBehavior AmmoBehavior { get { return _ammoBehavior; } }
	
	#region UnityMethods
	
	private void Awake()
	{
		InitializeFiringMode();
		InitializeProjectileType();
		InitializeAimingMode();
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
		
		if(AimingModeEnum != _previousAimingModeEnum)
		{
			InitializeAimingMode();
			_previousAimingModeEnum = AimingModeEnum;
		}
		
		if (AmmoBehaviorEnum != _previousAmmoBehaviorEnum)
		{
			InitializeAmmoBehaviour();
			_previousAmmoBehaviorEnum = AmmoBehaviorEnum;
		}
	}
	
	#endregion

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
		
		OnWeaponFireModeSwitchedEvent?.DynamicInvoke();
		OnWeaponFireModeSwitched?.Invoke();
	}

	public void Aim()
	{
		if (_aimingMode != null)
		{
			_aimingMode.Aim();
			GunDataRef.isAiming = true;
		}
	}
	
	public void Reload()
	{
		if (_ammoBehavior != null)
		{
			_ammoBehavior.Reload();
		}
	}

	public void StopAiming()
	{
		if (_aimingMode != null)
		{
			_aimingMode.StopAiming();
			GunDataRef.isAiming = false;
		}
	}
	
	public void SetAimingMode(IAimingMode mode)
	{
		this._aimingMode = mode;
	}
	
	public IAmmoBehavior GetCurrentAmmoBehaviour()
	{
		return _ammoBehavior;
	}
	
	public IAimingMode GetCurrentAimingMode()
	{
		return _aimingMode;
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
		else
		{
			Debug.LogError("Unsupported FireMode: " + FireModeEnum);
		}

		ComponentUtils.RemoveDuplicateComponents<IFiringMode>(gameObject);
	}
	
	private void InitializeAimingMode()
	{
		System.Type expectedType = GetAimingModeFromEnum();

		// Remove components that don't match the enum
		foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
		{
			if (component is IAimingMode && component.GetType() != expectedType)
			{
	            #if UNITY_EDITOR
				DestroyImmediate(component);
	            #else
				Destroy(component);
	            #endif
			}
		}

		// Initialize AimingMode based on the enum
		if (expectedType == typeof(FirstPersonAiming))
		{
			_aimingMode = ComponentUtils.AddOrGetComponent<FirstPersonAiming>(gameObject);
		}
		else
		{
			Debug.LogError("Unsupported AimingMode: " + AimingModeEnum);
		}

		ComponentUtils.RemoveDuplicateComponents<IAimingMode>(gameObject);
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
		else if (expectedType == typeof(CallOfDutyAmmoBehavior))
		{
			_ammoBehavior = ComponentUtils.AddOrGetComponent<CallOfDutyAmmoBehavior>(gameObject);
		}
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
				return typeof(CallOfDutyAmmoBehavior);
				
			default:
				return null;
		}
	}
	
	private Type GetAimingModeFromEnum()
	{
		switch (AimingModeEnum)
		{
		case EAimingMode.FirstPerson:
			return typeof(FirstPersonAiming);
			
		case EAimingMode.ThirdPerson:
			//return typeof(CallOfDutyAmmoBehavior);
				
		default:
			return null;
		}
	}
	
	#endregion
}
