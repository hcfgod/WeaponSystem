using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
	[SerializeField] private string _weaponName;
	[SerializeField] private int _minDamage;
	[SerializeField] private int _maxDamage;
	[SerializeField] private float _range;

	public virtual void Attack()
	{
		
	}

	public virtual void Equip()
	{
		
	}
	
	public virtual void Unequip()
	{
		
	}

    public void SetWeaponAttributes(string name, int minDamage, int maxDamage, float range)
    {
        _weaponName = name;
        _minDamage = minDamage;
        _maxDamage = maxDamage;
        _range = range;
    }

    public string GetWeaponName()
	{ return _weaponName; }
	
	public void SetWeaponName(string weaponName)
	{ _weaponName = weaponName; }
	
	public int GetMinDamage()
	{ return _minDamage; }
	
	public void SetMinDamage(int minDamage)
	{ _minDamage = minDamage; }
	
	public int GetMaxDamage()
	{ return _maxDamage; }
	
	public void SetMaxDamage(int maxDamage)
	{ _minDamage = maxDamage; }
	
	public float GetRange()
	{ return _range; }
	
	public void SetRange(float range)
	{ _range = range; }
}
