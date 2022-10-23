using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    public Item Item;
    public int Count;

    public ItemStack(Item item, int count)
    {
        Item = item;
        Count = count;
    }
}
