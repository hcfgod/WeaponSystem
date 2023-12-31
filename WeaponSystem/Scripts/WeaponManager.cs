﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public IWeapon CurrentWeapon { get; set; }


	private void Start()
	{
		SwitchWeapon(FindObjectOfType<RangedWeapon>());
	}
	
	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			FireCurrentWeapon();
		}
	}
	
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
