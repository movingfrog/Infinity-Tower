using UnityEngine;

public class SwordWeapon : Weapon
{
    public LayerMask EnemyLayer;

    public override void Attack()
    {
        TriggerHitEnchants();
        MoveWeapon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((EnemyLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.TryGetComponent<IHealth>(out var health))
            {
                health.Hurt(AttackDamageCaculator(PlayerStatManager.instance.damage + damage));
                TriggerAttackEnchant(collision.gameObject);
            }
        }
    }

    public override void EndAttack()
    {
        if (cooltimeCoroutine == null)
            cooltimeCoroutine = StartCoroutine(StartCooltime());
    }

    public override void PositionMove(Vector2 value, float attackRange) { }
}
