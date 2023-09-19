using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Behavior Data", menuName = "Weapon System/Ammo Behavior Data")]
public class AmmoBehaviorData : ScriptableObject
{
	public int MaxAmmo; // Maximum ammo capacity
	public int MagSize; // Magazine size
	public float ReloadTime;
}