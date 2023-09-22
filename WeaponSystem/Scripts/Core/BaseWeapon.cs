using UnityEngine;
using UnityEngine.Events;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
	#region Events
	
	public UnityEvent2 OnWeaponEquipped;
	
	public delegate void WeaponEquipped();
	public event WeaponEquipped OnWeaponEquippedEvent;
	
	public UnityEvent2 OnWeaponUnEquipped;	
	
	public delegate void WeaponUnEquipped();
	public event WeaponUnEquipped OnWeaponUnEquippedEvent;
	
	#endregion
	
	public WeaponData WeaponDataRef;

	public virtual void Attack(){}

	public virtual void Equip()
	{
		OnWeaponEquipped?.Invoke();
		OnWeaponEquippedEvent?.DynamicInvoke();
	}
	
	public virtual void Unequip()
	{
		OnWeaponUnEquipped?.Invoke();
		OnWeaponUnEquippedEvent?.DynamicInvoke();
	}
}
