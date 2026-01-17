using UnityEngine;

public class landEnemy : parentEnemy
{
    [Range(0f, 1f)]
    public float AttackDelay;
    public Vector2 attackSize;
    public Vector2 attackPosition;
    public LayerMask attackLayer;

    [Header("이동 관련")]
    public float Speed;
    public float groundCheckDistance;
    public float wallCheckDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    
    protected Rigidbody2D rigid;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        base.Awake();
    }

    protected override void Attack()
    {
        if (isDie) return;
        Collider2D player = Physics2D.OverlapBox(transform.position + (Vector3)attackPosition * transform.localScale.x, attackSize, 1, attackLayer);

        if(player != null && !isAttack)
        {
            isAttack = true;
            transform.localScale = new Vector2(transform.position.x - player.transform.position.x >= 0 ? -1 : 1, 1);
            ani.SetBool("isRun", false);
            ani.SetTrigger("isAttack");
        }
    }
    public void insertDamage()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position + (Vector3)attackPosition * transform.localScale.x, attackSize, 1, attackLayer);

        if(player != null)
        {
            IHealth PHealth = player.GetComponent<IHealth>();
            PHealth.Hurt(AttackDamage);
        }
    }
    public void resetAttack() => StartCoroutine(waitAttackCool(AttackDelay, () => isAttack = false));

    protected override void Move()
    {
        if (isAttack || isDie) return;
        ani.SetBool("isRun", true);
        rigid.linearVelocityX = Speed * transform.localScale.x;
        healthBar.MovePosition(transform.position);

        RaycastHit2D wallRay = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);
        RaycastHit2D groundRay = Physics2D.Raycast(transform.position + new Vector3(0, -.5f, 0) + new Vector3(transform.localScale.x, 0, 0), Vector2.down, groundCheckDistance, groundLayer);

        if (wallRay.collider != null || groundRay.collider == null) transform.localScale = new Vector2(-transform.localScale.x, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1,1,1,.3f);
        Ray wallRay = new Ray(transform.position, Vector2.right * transform.localScale.x * wallCheckDistance);
        Ray groundRay = new Ray(transform.position + new Vector3(0, -.5f, 0) + new Vector3(transform.localScale.x, 0, 0), Vector2.down * groundCheckDistance);
        Gizmos.DrawRay(wallRay);
        Gizmos.DrawRay(groundRay);
        Gizmos.DrawCube(transform.position + (Vector3)attackPosition * transform.localScale.x, attackSize);
    }
}
