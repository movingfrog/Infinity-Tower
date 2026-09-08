using UnityEngine;

public partial class Frog : OneAttackEnemy
{
    [SerializeField]
    private GameObject[] AttackPattern;

    [SerializeField]
    private float radius;

    [SerializeField]
    private LayerMask Player;

    Collider2D PlayerColl;
    bool isIn;

    public override void Attack()
    {
        if (isAttack || !isIn)
            return;

        TongueAttack();

        isAttack = true;
        resetAttack();
    }

    public override void Move()
    {
        Collider2D PColl = Physics2D.OverlapCircle(transform.position, radius, Player);

        if (PColl != null)
        {
            Collider2D APC =
                Physics2D.OverlapCircle(transform.position, radius * .5f, Player);

            if (APC == null)
            {
                rigid.linearVelocityX =
                    Speed * Mathf.Sign(PColl.transform.position.x - transform.position.x);

                healthBar.MovePosition(transform.position);
                isIn = false;
            }
            else
            {
                rigid.linearVelocityX = 0;
                PlayerColl = APC;
                isIn = true;
            }
        }
        else
        {
            rigid.linearVelocityX = 0;
            isIn = false;
        }
    }

    partial void TongueAttack();

    private void OnDrawGizmos()
    {
        Gizmos.color =
            isIn
                ? Color.white
                : Color.softRed * new Color(1, 1, 1, .3f);

        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius * .5f);
    }
}