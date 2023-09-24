using UnityEngine;
using UnityEngine.Events;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
	[Space(10)]
	
	public WeaponData WeaponDataRef;
	
	[Space(10)]
	
	#region Events
	
	public UnityEvent2 OnWeaponEquipped;
	
	public delegate void WeaponEquipped();
	public event WeaponEquipped OnWeaponEquippedEvent;
	
	public UnityEvent2 OnWeaponUnEquipped;	
	
	public delegate void WeaponUnEquipped();
	public event WeaponUnEquipped OnWeaponUnEquippedEvent;
	
	#endregion
	
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
