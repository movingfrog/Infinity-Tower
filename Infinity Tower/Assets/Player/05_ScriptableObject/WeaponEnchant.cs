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
    [field: SerializeField, TextArea]
    public string EnchantExplain { get; private set; }

    public abstract EnchantType Type { get; }

    /// <summary>
    /// 무기 관련 이벤트가 발생했을 때 호출될 메서드
    /// </summary>
    /// <param name="weapon">각인이 장착된 무기</param>
    /// <param name="target">타격당한 적 (Attack 타입일 때만 사용, 평소엔 null)</param>
    public abstract void WeaponUpgrade(Weapon weapon, GameObject target = null);
}
