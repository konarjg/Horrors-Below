using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon
{
    public ItemWeapon Stats;
    public int AmmunitionLoaded;
    public int Ammunition;
}

public enum WeaponType
{
    PISTOL,
    RIFLE,
    SHOTGUN,
    ENERGY
}

[CreateAssetMenu(menuName = "Horrors Below/Items/New Weapon")]
public class ItemWeapon : Item
{
    public WeaponType Type;
    public float Range;
    public float Damage;
    public float ReloadTime;
    public float FireRate;
    public float Recoil;
    public float Knockback;
    public int CapSize;
    public bool IsAutomatic;
}
