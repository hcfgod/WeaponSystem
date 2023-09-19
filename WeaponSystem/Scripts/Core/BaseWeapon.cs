using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
	public WeaponData WeaponDataRef;

	public virtual void Attack()
	{
		
	}

	public virtual void Equip()
	{
		
	}
	
	public virtual void Unequip()
	{
		
	}
}
