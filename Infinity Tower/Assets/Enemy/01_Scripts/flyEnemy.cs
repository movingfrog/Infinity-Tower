using UnityEngine;

public class flyEnemy : parentEnemy
{
    [Range(0f, 1f)]
    public float attackDelay;
    public float attackSize;
    public LayerMask attackLayer;

    [Header("이동 관련")]
    public float speed;
    public float observSize;


    protected override void Attack()
    {
        if (isDie) return;
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackSize, attackLayer);
        if(player != null && !isAttack)
        {
            isAttack = true;

        }
    }

    protected override void Move()
    {

    }
}
