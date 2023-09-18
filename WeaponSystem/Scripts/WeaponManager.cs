using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public IWeapon CurrentWeapon { get; set; }

	public void SwitchWeapon(IWeapon newWeapon)
	{
		CurrentWeapon?.Unequip();
		CurrentWeapon = newWeapon;
		CurrentWeapon.Equip();
	}

	public void FireCurrentWeapon()
	{
		CurrentWeapon?.Attack();
	}
}
