using UnityEngine;

public enum EnchantType
{
    Stat,
    Attack,
    Ability,
}

public abstract class WeaponEnchant : ScriptableObject
{
    /// <summary>
    /// 각인 능력치 상세 설명
    /// </summary>
    [TextArea]
    [field: SerializeField]
    public string EnchantExplain { get; private set; }

    public abstract EnchantType Type { get; }

    /// <summary>
    /// 무기 공격 시 실행되어서 추가 능력을 제공하는 메서드
    /// </summary>
    protected abstract void WeaponUpgrade();
}
