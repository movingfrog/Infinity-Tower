using UnityEngine;

public enum ModifierType
{
    Flat,
    PercentAdd,
    PercentMult,
}

[System.Serializable]
public class TechStatModifier
{
    public StatType statType;
    public ModifierType modType;
    public float value;

    public TechStatModifier(StatType _statType, ModifierType _modType, float _value)
    {
        statType = _statType;
        modType = _modType;
        value = _value;
    }
}

[CreateAssetMenu(
    fileName = "SO_StatTechData",
    menuName = "Scriptable Objects/TechData/SO_StatTech"
)]
public class SO_StatTechData : SO_TechData
{
    [field: SerializeField]
    TechStatModifier techStatModifier;
}
