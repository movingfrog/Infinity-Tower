using System.Collections;
using UnityEngine;

public class SwordWeapon : Weapon
{
    public LayerMask EnemyLayer;

    public override void Attack()
    {
        TriggerHitEnchants();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((EnemyLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (
                collision.TryGetComponent<parentEnemy>(out var health)
                && health.DamageWaitCoroutine == null
            )
            {
                health.DamageWaitCoroutine = StartCoroutine(DamageWait(attackRate, health));
                TriggerAttackEnchant(collision.gameObject);
            }
        }
    } // 맞는 대상을 통해서 공격 속도를 변경 필요

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((EnemyLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (
                collision.TryGetComponent<parentEnemy>(out var health)
                && health.DamageWaitCoroutine == null
            )
            {
                health.DamageWaitCoroutine = StartCoroutine(DamageWait(attackRate, health));
                TriggerAttackEnchant(collision.gameObject);
            }
        }
    } // n초당 한 번 공격하도록 변경

    private IEnumerator DamageWait(float time, parentEnemy health)
    {
        health.Hurt(AttackDamageCaculator(PlayerStatManager.instance.damage + damage));
        yield return new WaitForSeconds(time);
        health.DamageWaitCoroutine = null;
    }

    public override void EndAttack()
    {
        isPushing = false;
    }
}
