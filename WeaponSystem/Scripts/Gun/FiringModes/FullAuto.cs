using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FullAuto : MonoBehaviour, IFiringMode
{
	#region Events

	public UnityEvent2 OnWeaponFiredUnityEvent;
	public delegate void WeaponFiredEventHandler();
	public event WeaponFiredEventHandler OnWeaponFired;
	
	#endregion
	
	private float nextFireTime = 0f;

	public void ExecuteFiringSequence(WeaponData weaponData, GunData gunData, IProjectileType projectileType)
	{
		if (Time.time >= nextFireTime)
		{
			// Fire the weapon
			projectileType.Fire(weaponData);

			// Update the next fire time
			nextFireTime = Time.time + 1f / gunData.fireRate;
			
			OnWeaponFired?.DynamicInvoke();
			OnWeaponFiredUnityEvent?.Invoke();
		}
	}
}
