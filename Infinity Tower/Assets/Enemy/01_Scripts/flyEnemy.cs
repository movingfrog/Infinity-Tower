using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class flyEnemy : parentEnemy
{
    public float attackSize;
    public LayerMask attackLayer;
    [Foldout("원거리 공격")]
    public Transform attackPos;
    [Foldout("원거리 공격")]
    public GameObject attackBall;

    [Header("이동 관련")]
    public float speed;
    public float observSize;
    public Vector2 angle;
    public LayerMask observLayer;

    protected override void Attack()
    {
        if (isDie) return;
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackSize, attackLayer);
        if(player != null && !isAttack)
        {
            isAttack = true;
            angle = player.transform.position - transform.position;
            rigid.linearVelocity = Vector2.zero;
            ani.SetBool("isRun", false);
            ani.SetTrigger("isAttack");
        }
    }

    public void attackFuction()
    {
        GameObject CAttack = Instantiate(attackBall, attackPos);
        FireBall FB = CAttack.GetComponent<FireBall>();
        FB.Init(angle, AttackDamage);
    }

    protected override void Move()
    {
        if (isAttack || isDie) return;

        Collider2D player = Physics2D.OverlapCircle(transform.position, observSize, attackLayer);
        if(player != null)
        {
            angle = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, angle, 0.1f, observLayer);
            if(hit.collider != null)
            {
                ani.SetBool("isRun", false);
                rigid.linearVelocity = Vector3.zero;
                return;
            }
            ani.SetBool("isRun", true);
            rigid.linearVelocity = angle * speed;
            transform.localScale = new Vector3(rigid.linearVelocityX > 0 ? -1 : 1, 1, 1);
            healthBar.MovePosition(transform.position);
        }
    }

    public override void Die()
    {
        isDie = true;
        rigid.gravityScale = 1f;
        GetComponent<Collider2D>().isTrigger = true;
        Destroy(healthBar.gameObject);
        ani.SetTrigger("isDie");
        Destroy(gameObject, 1f);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1, 1, 1, .2f);
        Ray ray = new Ray(transform.position, angle);
        Gizmos.DrawRay(ray);
        Gizmos.DrawWireSphere(transform.position, observSize);
        Gizmos.DrawWireSphere(transform.position, attackSize);
    }
}
