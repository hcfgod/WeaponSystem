using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CallOfDutyAmmoBehavior : MonoBehaviour, IAmmoBehavior
{
	public UnityEvent2 OnReloadStarted;
	public UnityEvent2 OnReloadFinished;
	
	public delegate void OnReloadStartEventHandler();
	public delegate void OnReloadFinishedEventHandler();
	public event OnReloadStartEventHandler OnReloadStartedEvent;
	public event OnReloadFinishedEventHandler OnReloadFinishedEvent;
	
	[SerializeField] private AmmoBehaviorData ammoBehaviorData;
	[SerializeField] private GunData gunData;

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
		if(_currentAmmoInMagazine <= 0)
		{
			gunData.isGunMagEmpty = true;
			return true;
		}
		
		gunData.isGunMagEmpty = false;
		return true;
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
		AudioManager.instance.PlaySFX(gunData.reloadAudiop1, 0.5f, false);
		
		OnReloadStarted?.Invoke();
		OnReloadStartedEvent?.DynamicInvoke();
		
		StartCoroutine(ReloadRoutine());
	}
	
	public IEnumerator ReloadRoutine()
	{
		yield return new WaitForSeconds(ammoBehaviorData.reloadDelayForSecondPartOfAudio);
		
		AudioManager.instance.PlaySFX(gunData.reloadAudiop2, 0.5f, false);
		
		float timeLeftToWait = ammoBehaviorData.ReloadTime - ammoBehaviorData.reloadDelayForSecondPartOfAudio;
				
		yield return new WaitForSeconds(timeLeftToWait);

		LoadMagazineFromReserve();

		OnReloadFinished?.Invoke();
		OnReloadFinishedEvent?.DynamicInvoke();
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