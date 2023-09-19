using UnityEngine;

[CreateAssetMenu(fileName = "NewAmmoData", menuName = "Weapon System/AmmoData")]
public class AmmoData : ScriptableObject
{
	public int MaxAmmo; // Maximum ammo capacity
	public int MagSize; // Magazine size
}