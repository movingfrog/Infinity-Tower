using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { ATK, CRIT_RATE, CRIT_DMG, SPEED, GOLDBOOST, HEALBOOST }
public enum ItemLevel { Common, Rare, Legend }
public enum EquipType { Weapon, Accessorie }
[Serializable]
public class StatModifier
{
    public StatType Type;
    public float Value;
}
[Serializable]
public class EquipmentClass
{
    public EquipType type;
    [Foldout("무기 아이템")]
    public uint itemDamage;
    [Foldout("악세서리 아이템")]
    public List<StatModifier> statModifiers;
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("InventoryAttribute")]
    public Sprite spriteImage;
    public string itemName;
    public string itemInfo;
    public ItemLevel level;
    public int MaxItemCount;
    public bool isWearable;
    [Foldout("장비들")]
    public EquipmentClass Equips;
}
