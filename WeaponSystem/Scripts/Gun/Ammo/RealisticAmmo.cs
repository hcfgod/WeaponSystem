using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticAmmo : MonoBehaviour, IAmmoBehavior
{
	[SerializeField] private AmmoData ammoData;
	
	[SerializeField] private int _currentReserveAmmo = 0;
	[SerializeField] private int _currentMagAmmo = 0;
	
	public bool CanShoot()
	{
		return _currentMagAmmo > 0;
	}

	public void ConsumeAmmo()
	{
		if (_currentMagAmmo > 0)
		{
			_currentMagAmmo--;
		}
	}
}
