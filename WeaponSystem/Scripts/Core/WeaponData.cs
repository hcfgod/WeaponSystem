using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
	public string WeaponName;
	public int MinDamage;
	public int MaxDamage;
	public float Range;
}
