using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum StatType
{
    ATK,
    CRIT_RATE,
    CRIT_DMG,
    SPEED,
    GOLDBOOST,
    HEALBOOST,
}

public enum ItemLevel
{
    Common,
    Rare,
    Legend,
}

[Serializable]
public class StatModifier
{
    public StatType Type;
    public float Value;
}

[Serializable]
public class EquipmentClass
{
    public Item nextItem;

    [TextArea]
    public string anvilInfoLine;

    [Foldout("무기 아이템")]
    public uint itemDamage;

    [Foldout("악세서리 아이템")]
    public List<StatModifier> statModifiers;
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("아이템 속성")]
    public Sprite spriteImage;
    public string itemName;
    public string itemInfo;
    public ItemLevel level;
    public SlotType slotType;
    public int MaxItemCount;
    public bool isEquippable;

    [Foldout("장비들")]
    public EquipmentClass Equips;
}
