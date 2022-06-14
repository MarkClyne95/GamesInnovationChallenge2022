using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/Items", order = 1)]
public class ScriptableInteractableObject_Base : ScriptableObject
{
    public ItemType itemType;

    public ItemRarity Rarity;

    public int Score;
}

[Flags]
public enum ItemRarity
{
    Common = 0x000000,
    Uncommon = 0x000001,
    Rare = 0x000010,
    Epic = 0x000011,
    Legendary = 0x000100
}

[Flags]
public enum ItemType
{
    Barrel = 0x000000,
    Crate = 0x000001
}
