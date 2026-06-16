using UnityEngine;

public class SpearWeapon : Weapon
{
    public Vector2 attackSize;
    public LayerMask EnemyLayer;
    public Vector2 attackRange;

    public override void Attack()
    {
        float finalDamage = AttackDamageCaculator(PlayerStatManager.instance.damage + damage);
        ani.SetTrigger("Attack");
        endAttack = true;
        Vector3 SpearPosition = new Vector3(
            transform.parent.parent.position.x,
            transform.position.y,
            0
        );
        Collider2D[] _EnemyColl = Physics2D.OverlapBoxAll(
            SpearPosition + computeAttackRange(),
            attackSize,
            ani.GetInteger("Y") == 0 ? 0 : 90,
            EnemyLayer
        );
        foreach (var enemy in _EnemyColl)
        {
            if (enemy.TryGetComponent<IHealth>(out var health))
            {
                health.Hurt(finalDamage);
            }
        }
    }

    public override void EndAttack()
    {
        if (cooltimeCoroutine == null)
            cooltimeCoroutine = StartCoroutine(StartCooltime());
    }

    public override void PositionMove(Vector2 value, float attackRange)
    {
        float ValueX = 1 - Mathf.Abs(GetSign(value.y));
        transform.parent.localPosition = new Vector2(
            attackRange * ValueX,
            attackRange * GetSign(value.y)
        );
        ani.SetInteger("X", (int)(ValueX * transform.parent.parent.localScale.x));
        ani.SetInteger("Y", (int)GetSign(value.y));
        if (value.y < 0)
            transform.localScale = baseScale * new Vector2(1, -1);
        else
            transform.localScale = baseScale * new Vector2(ValueX != 0 ? 1 : -1, 1);
    }

    private Vector3 computeAttackRange() //무기 이미지에 따른 무기 공격 위치의 중심 점 반환
    {
        Vector3 weaponPosition = transform.parent.localPosition;
        float x = GetSign(weaponPosition.x) * attackRange.x * 2;
        float y = GetSign(weaponPosition.y) * attackRange.y * 2;
        Vector3 position = new Vector3(
            (weaponPosition.x + x) * transform.parent.parent.localScale.x,
            weaponPosition.y + y,
            0
        );
        return position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1, 1, 1, .1f);
        Vector2 vec =
            GetSign(transform.parent.localPosition.y) == 0
                ? attackSize
                : new Vector2(attackSize.y, attackSize.x);
        Vector3 currentPosition = new Vector3(
            transform.parent.parent.position.x,
            transform.position.y,
            0
        );
        Gizmos.DrawCube(currentPosition + computeAttackRange(), vec);
    }
}
