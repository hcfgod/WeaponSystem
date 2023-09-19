using UnityEngine;

public class SingleShot : MonoBehaviour, IFiringMode
{
	public void ExecuteFiringSequence(WeaponData weaponData, GunData gunData, IProjectileType projectileType)
	{
		projectileType.Fire(weaponData);
	}
}
