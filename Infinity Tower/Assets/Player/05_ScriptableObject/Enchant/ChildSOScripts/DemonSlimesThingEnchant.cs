using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "DemonSlime's_Thing",
    menuName = "WeaponEnchantData/DemonSlime's_Thing"
)]
public class DemonSlimesThingEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const float TickRate = .15f;
    private const float DamageValue = .2f;
    private const int TickAmount = 5;

    public override void WeaponUpgrade(Weapon weapon, GameObject target = null)
    {
        float Damage = weapon.damage * DamageValue;
        float HealAmount = weapon.damage;

        weapon.StartCoroutine(InfernoAttack(Damage, target));
        PlayerStatManager.instance.ChangeHealth(HealAmount);
    }

    IEnumerator InfernoAttack(float Damage, GameObject target)
    {
        IHealth Enemy = target.GetComponent<IHealth>();
        if (Enemy == null)
            yield break;
        for (int i = 0; i < TickAmount; i++)
        {
            Enemy.Hurt(Damage);
            yield return new WaitForSeconds(TickRate);
        }
    }
}
