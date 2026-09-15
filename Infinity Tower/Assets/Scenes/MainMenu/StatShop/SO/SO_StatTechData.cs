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

    public override string GetEffect(int level)
    {
        if (level <= 0)
            Debug.LogError("값이 정상적이지 않습니다");

        return string.Format(
            _effect,
            (level - 1) * techStatModifier.value,
            level * techStatModifier.value
        );
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, level * techStatModifier.value);
    }

    public override void Apply(int level)
    {
        float finalValue = level * techStatModifier.value;
        var runtimeModifier = new TechStatModifier(
            techStatModifier.statType,
            techStatModifier.modType,
            finalValue
        );

        PlayerStatManager.instance.ApplyToStat(runtimeModifier);
    }
}
