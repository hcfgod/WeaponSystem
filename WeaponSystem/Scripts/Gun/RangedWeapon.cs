using UnityEngine;

public class RangedWeapon : BaseGun
{
	private void Start()
	{
		if(_projectileType == null)
			_projectileType = GetComponent<RaycastProjectile>();
	}
}