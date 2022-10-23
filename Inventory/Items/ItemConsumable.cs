using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    BLOOD,
    SANITY,
    STAMINA
}

[CreateAssetMenu(menuName = "Horrors Below/Items/New Consumable")]
public class ItemConsumable : Item
{
    public ConsumableType Type;
    public float RestoreAmount;
}
