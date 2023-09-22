using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon System/Ammo/Ammo Type Data")]
public class AmmoTypeData : ScriptableObject
{
    public EAmmoType AmmoType;
    public float DamageModifier;
    public float RangeModifier;
    // Add more properties as needed
}