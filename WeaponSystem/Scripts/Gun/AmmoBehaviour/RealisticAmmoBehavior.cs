using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticAmmoBehavior : MonoBehaviour, IAmmoBehavior
{
	[SerializeField] private AmmoBehaviorData ammoBehaviorData;
	
	private int _magazineCapacity;
	private int _reserveAmmoCapacity;
	
	[SerializeField] private int _currentAmmoInMagazine;
	[SerializeField] private int _currentReserveAmmo;
	
	private bool _isRoundChambered;

	private void Start()
	{
		_magazineCapacity = ammoBehaviorData.MagSize;
		_reserveAmmoCapacity = ammoBehaviorData.MaxAmmo;
		
		_currentAmmoInMagazine = _magazineCapacity;
		_reserveAmmoCapacity = _reserveAmmoCapacity;
		_isRoundChambered = true;
	}

	public bool CanShoot()
	{
		if (_currentAmmoInMagazine > 0 && _isRoundChambered)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void ConsumeAmmo()
	{
		if (_currentAmmoInMagazine > 0)
		{
			_currentAmmoInMagazine--;
		}
	}

	public void Reload()
	{
		StartCoroutine(ReloadRoutine());
	}
	
	private IEnumerator ReloadRoutine()
	{
		// Simulate reload time
		yield return new WaitForSeconds(ammoBehaviorData.ReloadTime);

		int ammoNeeded = _magazineCapacity - _currentAmmoInMagazine;

		if (_currentReserveAmmo >= ammoNeeded)
		{
			_currentAmmoInMagazine = _magazineCapacity;
			_currentReserveAmmo -= ammoNeeded;
		}
		else
		{
			_currentAmmoInMagazine += _currentReserveAmmo;
			_currentReserveAmmo = 0;
		}

		_isRoundChambered = true;
	}
}
