using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType { CRIT_RATE, CRIT_DMG, ATK, GOLD_RATE, SPEED, HEALBOOST }
[Serializable]
public class StatModifier
{
    public StatType Type;
    public float Value;
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 0)]
public class Item : ScriptableObject
{
    [Header("InventoryAttribute")]
    public Sprite spriteImage;
    public string itemInfo;
    public uint price;
    [Foldout("무기 아이템")]
    public uint itemDamage;
    [Foldout("악세서리 아이템")]
    public List<StatModifier> statModifiers;
}
