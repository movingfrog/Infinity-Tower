using UnityEngine;

[CreateAssetMenu(fileName = "SlowEnchant", menuName = "WeaponEnchantData/SlowEnchant")]
public class SlowEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const float SpeedDownValue = 1.0f - .3f;

    public override void WeaponUpgrade(Weapon weapon, GameObject target = null)
    {
        if (target.TryGetComponent(out IMove move))
        {
            move.Speed = move.Speed * SpeedDownValue;
        }
    }
}
