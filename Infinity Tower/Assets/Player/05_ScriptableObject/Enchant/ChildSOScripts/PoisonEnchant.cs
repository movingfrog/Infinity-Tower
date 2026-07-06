using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonEnchant", menuName = "WeaponEnchantData/PoisonEnchant")]
public class PoisonEnchant : WeaponEnchant
{
    public override EnchantType Type => EnchantType.Attack;

    private const float PoisonDamage = 2;
    private const float TickTime = .5f;
    private const int TickAmount = 3;

    protected override void WeaponUpgrade() => Debug.Log("독 공격 실행");

    public void WeaponUpgrade(IHealth enemy, int weaponLevel, MonoBehaviour runner)
    {
        WeaponUpgrade();
        runner.StartCoroutine(PoisonAttack(weaponLevel * PoisonDamage, enemy));
    }

    IEnumerator PoisonAttack(float Damage, IHealth Enemy)
    {
        for (int i = 0; i < TickAmount; i++)
        {
            Enemy.Hurt(Damage);
            yield return new WaitForSeconds(TickTime);
        }
    }
}
