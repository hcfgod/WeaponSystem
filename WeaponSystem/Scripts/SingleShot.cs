using UnityEngine;

public class SingleShot : MonoBehaviour, IFiringMode
{
	public void ExecuteFiringSequence(WeaponData weaponData, IProjectileType projectileType)
	{
		projectileType.Fire(weaponData);
	}
}
