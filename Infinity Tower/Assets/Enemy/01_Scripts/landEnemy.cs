using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Jobs;

public class landEnemy : parentEnemy
{
    [Header("이동 관련")]
    public float Speed;
    public float groundCheckDistance;
    public float wallCheckDistance;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    
    [Header("공격 관련")]
    public Vector2 attackSize;
    public Vector2 attackPosition;
    public LayerMask attackLayer;

    protected Rigidbody2D rigid;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        base.Awake();
    }

    private void FixedUpdate()
    {
        Attack();
        Move();
    }

    protected override void Attack()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position + (Vector3)attackPosition * transform.localScale.x + Vector3.down * .5f, attackSize, 1, attackLayer);

        if(player != null && !isAttack)
        {
            isAttack = true;
            transform.localScale = new Vector2(transform.position.x - player.transform.position.x >= 0 ? -1 : 1, 1);
            ani.SetTrigger("isAttack");
        }
    }

    protected override void Move()
    {
        if (isAttack) return;
        rigid.linearVelocity = Vector2.right * Speed * transform.localScale.x;

        RaycastHit2D wallRay = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);
        RaycastHit2D groundRay = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0) + new Vector3(transform.localScale.x, 0, 0), Vector2.down, groundCheckDistance, groundLayer);

        if (wallRay.collider != null || groundRay.collider == null) transform.localScale = new Vector2(-transform.localScale.x, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1,1,1,.3f);
        Ray wallRay = new Ray(transform.position, Vector2.right * transform.localScale.x * wallCheckDistance);
        Ray groundRay = new Ray(transform.position + new Vector3(0, -1, 0) + new Vector3(transform.localScale.x, 0, 0), Vector2.down * groundCheckDistance);
        Gizmos.DrawRay(wallRay);
        Gizmos.DrawRay(groundRay);
        Gizmos.DrawCube(transform.position + (Vector3)attackPosition * transform.localScale.x + Vector3.down * .5f, attackSize);
    }
}
