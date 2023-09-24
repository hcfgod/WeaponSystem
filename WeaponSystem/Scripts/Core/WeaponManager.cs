using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponManager : MonoBehaviour
{
	[SerializeField] PlayerData playerData;
	
	public IWeapon CurrentWeapon { get; set; }
	
	private WeaponAnimator _gunAnimator;
	
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
		
		if(CurrentWeapon is BaseGun gun)
		{
			_gunAnimator = gun.gameObject.GetComponentInChildren<WeaponAnimator>();

			_gunAnimator.GunData = gun.GunDataRef;
		}
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
			
			// Aim when the right mouse button is pressed
			if (Input.GetMouseButton(1))
			{
				if(!playerData.canAim)
					return;
					
				gun.Aim();
			}
			else
			{
				if(!playerData.canAim)
					return;
					
				gun.StopAiming();
			}

			if(Input.GetKeyDown(KeyCode.R))
			{
				gun.Reload();
			}
			
			if(gun.GetCurrentAmmoBehaviour() is RealisticAmmoBehavior realisticAmmoBehaviour)
			{
				if (Input.GetKeyDown(KeyCode.C))
				{
					realisticAmmoBehaviour.ChamberRound();
				}
			}
		}
	}
}
