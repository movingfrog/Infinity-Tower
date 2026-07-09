using UnityEngine;

[CreateAssetMenu(fileName = "SwallowEnchant", menuName = "WeaponEnchantData/SwallowEnchant")]
public class SwallowEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const int percent = 30;
    private const float HealValue = .1f;

    public override void WeaponUpgrade(Weapon weapon, GameObject target = null)
    {
        int randValue = Random.Range(0, 100);
        if (percent >= randValue)
        {
            float HealAmount = weapon.damage * (HealValue * (int)(1 + weapon.Level));
            PlayerStatManager.instance.ChangeHealth(HealAmount);
        }
    }
}
