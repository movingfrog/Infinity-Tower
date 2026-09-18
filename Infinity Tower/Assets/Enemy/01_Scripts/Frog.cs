using System.Collections;
using UnityEngine;

public class Frog : OneAttackEnemy
{
    [Header("Range")]
    [SerializeField]
    private Vector2 moveRange;

    [SerializeField]
    private Vector2 attackRange;

    [Header("Target")]
    [SerializeField]
    private LayerMask Player;

    [Header("Component")]
    [SerializeField]
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private float direction = 1f;


    public override void Attack()
    {
        if (isAttack)
            return;

        Vector2 center = transform.position;
        center.x += direction * attackRange.x * 0.5f;

        Collider2D PColl =
            Physics2D.OverlapBox(
                center,
                attackRange,
                0f,
                Player
            );

        if (PColl == null)
            return;

        isAttack = true;

        animator.SetTrigger("Attack");
    }


    public override void Move()
    {
        if (isAttack)
        {
            rigid.linearVelocityX = 0;

            animator.SetBool("IsRun", false);

            return;
        }

        Collider2D PColl =
            Physics2D.OverlapBox(
                transform.position,
                moveRange,
                0f,
                Player
            );

        if (PColl != null)
        {
            
            float moveDirection =
                Mathf.Sign(
                    PColl.transform.position.x
                    - transform.position.x
                );

            rigid.linearVelocityX =
                Speed * moveDirection;
            
            direction = moveDirection;
            
            spriteRenderer.flipX =
                direction < 0;
    
            animator.SetBool("IsRun", true);

            healthBar.MovePosition(
                transform.position
            );
        }
        else
        {
            rigid.linearVelocityX = 0;

            animator.SetBool("IsRun", false);
        }
    }
    public void Hit()
    {
        Vector2 center = transform.position;
        center.x += direction * attackRange.x * 0.5f;

        Collider2D PColl =
            Physics2D.OverlapBox(
                center,
                attackRange,
                0f,
                Player
            );

        if (PColl == null)
            return;

        IHealth health =
            PColl.GetComponent<IHealth>();

        if (health != null)
            health.Hurt(AttackDamage);

        PlayerStatManager.instance.StartCoroutine(
            SlowPlayer()
        );
    }


    private IEnumerator SlowPlayer()
    {
        PlayerStatManager.instance.statUp(
            StatType.SPEED,
            -15
        );

        yield return new WaitForSeconds(3f);

        PlayerStatManager.instance.statUp(
            StatType.SPEED,
            15
        );
    }


    public void EndAttack()
    {
        resetAttack();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(
            transform.position,
            moveRange
        );


        Vector2 center = transform.position;
        center.x += direction * attackRange.x * 0.5f;

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(
            center,
            attackRange
        );
    }
}
