using UnityEngine;
using System.Collections;

public class CallOfDutyAmmoBehavior : MonoBehaviour, IAmmoBehavior
{
	[SerializeField] private AmmoBehaviorData ammoBehaviorData;

	private int _magazineCapacity;
	private int _reserveAmmoCapacity;
	
	[SerializeField] private int _currentAmmoInMagazine;
	[SerializeField] private int _currentReserveAmmo;

	private void Start()
	{
		_magazineCapacity = ammoBehaviorData.MagSize;
		_reserveAmmoCapacity = ammoBehaviorData.MaxAmmo;
		
		_currentAmmoInMagazine = _magazineCapacity;
		_currentReserveAmmo = _reserveAmmoCapacity;
	}

	public bool CanShoot()
	{
		return _currentAmmoInMagazine > 0;
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
	
	public IEnumerator ReloadRoutine()
	{
		// Simulate reload time
		yield return new WaitForSeconds(ammoBehaviorData.ReloadTime);

		LoadMagazineFromReserve();
	}
	
	public void AddAmmoToReserve(int amount)
	{
		_currentReserveAmmo += amount;
		if (_currentReserveAmmo > _reserveAmmoCapacity)
		{
			_currentReserveAmmo = _reserveAmmoCapacity;
		}
	}

	public void LoadMagazineFromReserve()
	{
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
	}
}