using UnityEngine;
using UnityEngine.Events;

public class SingleShot : MonoBehaviour, IFiringMode
{
	#region Events

	public UnityEvent2 OnWeaponFired;
	public delegate void WeaponFiredEventHandler();
	public event WeaponFiredEventHandler OnWeaponFiredEvent;
	
	#endregion
	
	public void ExecuteFiringSequence(WeaponData weaponData, GunData gunData, IProjectileType projectileType)
	{
		projectileType.Fire(weaponData);
		
		OnWeaponFiredEvent?.DynamicInvoke();
		OnWeaponFired?.Invoke();
	}
}
