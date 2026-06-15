using UnityEngine;

public class guardEnemy : OneAttackEnemy
{
    public Vector2 attackSize;
    public Vector2 attackPos;
    public LayerMask TargetLayer;
    public bool isIn;

    [Header("이동 관련")]
    public float moveSize;
    Vector3 startPos;

    protected override void Awake()
    {
        startPos = transform.position;
        base.Awake();
    }

    public override void Attack()
    {
        if (isDie || !isIn)
            return;
        Collider2D player = Physics2D.OverlapBox(
            transform.position + (Vector3)attackPos * transform.localScale.x,
            attackSize,
            1,
            TargetLayer
        );
        if (player != null && !isAttack)
        {
            isAttack = true;
            transform.localScale = new Vector2(
                transform.position.x - player.transform.position.x >= 0 ? 1 : -1,
                1
            );
            rigid.linearVelocity = Vector2.zero;
            ani.SetBool("isRun", false);
            ani.SetTrigger("isAttack");
        }
    }

    public void insertDamage()
    {
        Collider2D player = Physics2D.OverlapBox(
            transform.position + (Vector3)attackPos * transform.localScale.x,
            attackSize,
            1,
            TargetLayer
        );

        if (player != null)
        {
            IHealth PHealth = player.GetComponent<IHealth>();
            PHealth.Hurt(AttackDamage);
        }
    }

    public override void Move()
    {
        if (isAttack || isDie)
            return;
        Collider2D player = Physics2D.OverlapCircle(startPos, moveSize, TargetLayer);
        if (player != null)
        {
            isIn = true;
            ani.SetBool("isRun", true);
            Vector3 targetPos = player.transform.position - transform.position;
            transform.localScale = new Vector3(targetPos.x >= 0 ? -1 : 1, 1, 1);
            rigid.linearVelocity = targetPos;
        }
        else
        {
            isIn = false;
            if ((startPos - transform.position).magnitude > .1f)
            {
                Vector3 startAngle = startPos - transform.position;
                transform.localScale = new Vector3(startAngle.x >= 0 ? -1 : 1, 1, 1);
                rigid.linearVelocity = startAngle;
            }
            else
                ani.SetBool("isRun", false);
        }
        healthBar.MovePosition(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, .3f) * Color.green;
        Gizmos.DrawWireCube(
            transform.position + (Vector3)attackPos * transform.localScale.x,
            attackSize
        );
        Gizmos.DrawWireSphere(startPos, moveSize);
    }
}
