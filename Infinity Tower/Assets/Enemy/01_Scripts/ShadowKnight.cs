using UnityEngine;

public class ShadowKnight : OneAttackEnemy
{
    [Header("이동 관련")]
    [SerializeField]
    private float PSpeed;

    [SerializeField]
    private Vector2 SerchingSize;

    [Space(10f)]
    [SerializeField]
    private float groundCheckDistance;

    [SerializeField]
    private float wallCheckDistance;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private LayerMask wallLayer;

    [Header("공격 관련")]
    [SerializeField]
    private Vector2 AttackSize;

    [SerializeField]
    private Vector2 AttackCenter;

    [SerializeField]
    private LayerMask PlayerLayer;

    private SpriteRenderer SR;

    protected override void Awake()
    {
        base.Awake();
        SR = GetComponent<SpriteRenderer>();
    }

    public override void Attack()
    {
        if (isDie)
            return;
        Collider2D PColl = Physics2D.OverlapBox(
            transform.position
                + (Vector3)AttackCenter * transform.localScale.x
                + Vector3.down * .5f,
            AttackSize,
            0,
            PlayerLayer
        );
        if (PColl != null && !isAttack)
        {
            isAttack = true;
            transform.localScale = new Vector2(
                transform.position.x - PColl.transform.position.x >= 0 ? -1 : 1,
                1
            );
            rigid.linearVelocity = Vector2.zero;
            ani.SetBool("IsRun", false);
            ani.SetTrigger("isAttack");
        }
    }

    public void InsertDamage()
    {
        Collider2D PColl = Physics2D.OverlapBox(
            transform.position
                + (Vector3)AttackCenter * transform.localScale.x
                + Vector3.down * .5f,
            AttackSize,
            0,
            PlayerLayer
        );
        if (PColl != null && PColl.TryGetComponent(out IHealth PHealth))
        {
            PHealth.Hurt(AttackDamage);
        }

        resetAttack();
    }

    public override void Move()
    {
        if (isAttack || isDie)
            return;
        RaycastHit2D wallRay = Physics2D.Raycast(
            transform.position + Vector3.down * .5f,
            Vector2.right * transform.localScale.x,
            wallCheckDistance,
            wallLayer
        );
        RaycastHit2D groundRay = Physics2D.Raycast(
            transform.position
                + new Vector3(0, -.5f, 0)
                + new Vector3(transform.localScale.x, 0, 0),
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (wallRay.collider != null || groundRay.collider == null)
            transform.localScale = new Vector2(-transform.localScale.x, 1);

        Collider2D PColl = Physics2D.OverlapBox(transform.position, SerchingSize, 0, PlayerLayer);
        float moveSpeed;
        if (PColl != null)
        {
            ani.SetBool("IsRun", true);
            transform.localScale = new Vector2(
                transform.position.x - PColl.transform.position.x >= 0 ? -1 : 1,
                1
            );
            SR.color = Color.white;
            moveSpeed = Speed + PSpeed;
        }
        else
        {
            ani.SetBool("IsRun", false);
            SR.color = Color.white * new Color(1, 1, 1, .2f);
            moveSpeed = Speed;
        }
        rigid.linearVelocityX = moveSpeed * transform.localScale.x;
        healthBar.MovePosition(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.orangeRed * new Color(1, 1, 1, .3f);

        Gizmos.DrawWireCube(transform.position, SerchingSize);
        Gizmos.DrawWireCube(
            transform.position
                + (Vector3)AttackCenter * transform.localScale.x
                + Vector3.down * .5f,
            AttackSize
        );

        Ray wallRay = new Ray(
            transform.position + Vector3.down * .5f,
            Vector2.right * transform.localScale.x * wallCheckDistance
        );
        Ray groundRay = new Ray(
            transform.position
                + new Vector3(0, -.5f, 0)
                + new Vector3(transform.localScale.x, 0, 0),
            Vector2.down * groundCheckDistance
        );
        Gizmos.DrawRay(wallRay);
        Gizmos.DrawRay(groundRay);
    }
}
