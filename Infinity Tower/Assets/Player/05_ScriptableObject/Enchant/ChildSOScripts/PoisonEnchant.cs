using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEnchant", menuName = "WeaponEnchantData/PoisonEnchant")]
public class PoisonEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const float PoisonDamage = 2;
    private const float TickTime = .5f;
    private const int TickAmount = 3;

    public override void WeaponUpgrade(Weapon weapon, GameObject target = null)
    {
        weapon.StartCoroutine(PoisonAttack((int)(weapon.Level + 1) * PoisonDamage, target));
    }

    IEnumerator PoisonAttack(float Damage, GameObject target)
    {
        IHealth Enemy = target.GetComponent<IHealth>();
        if (Enemy == null)
            yield break;
        for (int i = 0; i < TickAmount; i++)
        {
            Enemy.Hurt(Damage);
            yield return new WaitForSeconds(TickTime);
        }
    }
}
