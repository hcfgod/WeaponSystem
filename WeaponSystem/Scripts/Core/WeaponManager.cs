using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour
{
	public IWeapon CurrentWeapon { get; set; }

	private void Start()
	{
		// Temp Setting The Current Weapon To A Test Weapon For Testing Purposes
		SwitchWeapon(FindObjectOfType<RangedWeapon>());
	}
	
	private void Update()
	{
		HandleGunInput();
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
	
	public System.Type GetCurrentWeaponType()
	{
		if (CurrentWeapon == null)
		{
			return null;
		}
        
		return CurrentWeapon.GetType();
	}
	
	private void HandleGunInput()
	{
		if (CurrentWeapon is BaseGun gun)
		{
			if(gun.GetCurrentAmmoBehaviour() is RealisticAmmoBehavior realisticAmmoBehaviour)
			{
				if (Input.GetKeyDown(KeyCode.C))
				{
					realisticAmmoBehaviour.ChamberRound();
				}
			}
			
			if (gun.FireModeEnum == EFireMode.Auto)
			{
				if (Input.GetMouseButton(0))
				{
					CurrentWeapon.Attack();
				}
			}
			else if(gun.FireModeEnum == EFireMode.Single)
			{
				if (Input.GetMouseButtonDown(0))
				{
					CurrentWeapon.Attack();
				}
			}
			
			if(Input.GetKeyDown(KeyCode.R))
			{
				gun.AmmoBehavior.Reload();
			}
		}
	}
}
