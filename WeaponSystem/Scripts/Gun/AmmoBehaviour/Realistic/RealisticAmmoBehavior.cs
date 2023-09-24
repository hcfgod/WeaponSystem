using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RealisticAmmoBehavior : MonoBehaviour, IAmmoBehavior
{
	[SerializeField] private AmmoBehaviorData ammoBehaviorData;
	
	[SerializeField] private List<EAmmoType> supportedAmmoTypes;
	[SerializeField] private EAmmoType currentAmmoType;
	
	[SerializeField] private bool _requiresManualBolting = false;
	
	private int _magazineCapacity;
	private int _reserveAmmoCapacity;
	
	private int _currentAmmoInChamber;
	private int _currentAmmoInMagazine;
	private int _currentReserveAmmo;

	private bool _isChambering = false;
	private bool _isRoundChambered = false;
	
	private bool isReloading = false;

	#region Events

	public UnityEvent2 OnRoundChambered;
	public delegate void RoundChamberedEventHandler();
	public event RoundChamberedEventHandler OnRoundChamberedEvent;
	
	public UnityEvent2 OnReloadStarted;
	public UnityEvent2 OnReloadFinished;
	
	public delegate void OnReloadStartEventHandler();
	public delegate void OnReloadFinishedEventHandler();
	public event OnReloadStartEventHandler OnReloadStartedEvent;
	public event OnReloadFinishedEventHandler OnReloadFinishedEvent;
	
	#endregion
	
	private void Start()
	{
		_magazineCapacity = ammoBehaviorData.MagSize;
		_reserveAmmoCapacity = ammoBehaviorData.MaxAmmo;
		
		_currentAmmoInMagazine = _magazineCapacity;
		_currentReserveAmmo = _reserveAmmoCapacity;
	}

	public bool CanShoot()
	{
		if(_isChambering) return false;

		if(!_isRoundChambered) return false;
		
		if(isReloading) return false;
		
		return true;
	}

	public void ConsumeAmmo()
	{
		if (_isRoundChambered)
		{
			_currentAmmoInChamber--;
			_isRoundChambered = false;
			
			if(!_requiresManualBolting)
				ChamberRound();
		}
	}

	public void Reload()
	{
		if(_currentReserveAmmo <= 0) return;
		
		isReloading = true;
		
		StartCoroutine(ReloadRoutine());

		OnReloadStarted?.Invoke();
		OnReloadStartedEvent?.DynamicInvoke();
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
		if(isReloading)
			return;
			
		StartCoroutine(ChamberRoundRoutine());
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
	
	private IEnumerator ChamberRoundRoutine()
	{
		_isChambering = true;
		
		yield return new WaitForSeconds(ammoBehaviorData.ChamberingDelay);
		
		if (!_isRoundChambered && _currentAmmoInMagazine > 0)
		{
			_currentAmmoInMagazine--;
			_currentAmmoInChamber++;
        
			_isRoundChambered = true;
			
			OnRoundChambered?.Invoke();
			OnRoundChamberedEvent?.DynamicInvoke();
		}

		_isChambering = false;
	}
	
	private IEnumerator ReloadRoutine()
	{
		yield return new WaitForSeconds(ammoBehaviorData.ReloadTime);

		LoadMagazineFromReserve();
		
		_isRoundChambered = false;
		isReloading = false;
		
		OnReloadFinished?.Invoke();
		OnReloadFinishedEvent?.DynamicInvoke();
	}
	
	public bool GetIsReloading()
	{
		return isReloading;
	}
}
