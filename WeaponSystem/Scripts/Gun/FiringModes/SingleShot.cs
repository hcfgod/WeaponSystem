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
		if(gunData.isGunMagEmpty)
		{
			AudioManager.instance.PlaySFX(gunData.emptyshootingAudio, 0.5f, true);
			return;
		}
		
		projectileType.Fire(weaponData);
		
		AudioManager.instance.PlaySFX(gunData.shootingAudio, 0.5f, true);
			
		OnWeaponFiredEvent?.DynamicInvoke();
		OnWeaponFired?.Invoke();
	}
}
