using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InfernoEnchant", menuName = "WeaponEnchantData/InfernoEnchant")]
public class InfernoEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const float InfernoDamage = 3.0f;
    private const float TickRate = .15f;
    private const int TickAmount = 3;

    public override void WeaponUpgrade(Weapon weapon, GameObject target = null)
    {
        weapon.StartCoroutine(InfernoAttack((int)(weapon.Level + 1) * InfernoDamage, target));
    }

    IEnumerator InfernoAttack(float Damage, GameObject target)
    {
        if (target == null)
            yield break;
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
