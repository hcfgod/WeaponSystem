using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Weapon System/GunData", order = 0)]
public class GunData : ScriptableObject
{
	public float reloadTime;
	public int magazineSize;
	public float fireRate;
}
