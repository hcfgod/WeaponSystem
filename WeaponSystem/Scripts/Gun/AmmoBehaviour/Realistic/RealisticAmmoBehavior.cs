using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticAmmoBehavior : MonoBehaviour, IAmmoBehavior
{
	[SerializeField] private AmmoBehaviorData ammoBehaviorData;
	
	[SerializeField] private List<EAmmoType> supportedAmmoTypes;
	[SerializeField] private EAmmoType currentAmmoType;
	
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
			_isRoundChambered = false;
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

		_isRoundChambered = false;
	}
	
	public void ChangeAmmoType(EAmmoType newAmmoType)
	{
		if (supportedAmmoTypes.Contains(newAmmoType))
		{
			currentAmmoType = newAmmoType;
			// Update ammo properties based on the new type
		}
		else
		{
			Debug.LogWarning("This gun does not support the selected ammo type.");
		}
	}
	
	public void ChamberRound()
	{
		if (!_isRoundChambered && _currentAmmoInMagazine > 0)
		{
			_isRoundChambered = true;
			_currentAmmoInMagazine--;  // One round is moved from the magazine to the chamber
		}
		else
		{
			Debug.LogWarning("Cannot chamber a round.");
		}
	}
}
