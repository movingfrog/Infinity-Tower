using UnityEngine;

public class GhostKnight : OneAttackEnemy
{
    [Space(20f), SerializeField]
    private LayerMask PlayerLayer;

    [Header("이동 관련")]
    [SerializeField]
    private Vector2 MoveSize;

    [SerializeField]
    private float MoveCenter;

    [Header("공격 관련")]
    [SerializeField]
    private GhostKnightAttack GhostKnightAttacker;

    [SerializeField]
    private Vector2 AttackSize;

    [SerializeField]
    private Vector2 AttackCenter;

    protected override void Awake()
    {
        base.Awake();
        if (GhostKnightAttacker == null)
            Debug.LogError("GhostKnightAttacker is null");
        GhostKnightAttacker.TryGetComponent(out ani);
        rigid = GhostKnightAttacker.GetComponent<Rigidbody2D>();
        if (rigid == null)
            Debug.LogError("GhostKnightAttacker's Rigidbody2D is null");
        GhostKnightAttacker.onAttackEvent.AddListener(InsertDamage);
    }

    public override void Attack()
    {
        if (isDie)
            return;

        Collider2D PColl = Physics2D.OverlapBox(
            GhostKnightAttacker.transform.position
                + (Vector3)AttackCenter * GhostKnightAttacker.transform.localScale.x,
            AttackSize,
            0,
            PlayerLayer
        );
        if (PColl != null && !isAttack)
        {
            isAttack = true;
            rigid.linearVelocity = Vector2.zero;
            ani.SetTrigger("isAttack");
        }
    }

    public void InsertDamage()
    {
        Collider2D PColl = Physics2D.OverlapBox(
            GhostKnightAttacker.transform.position
                + (Vector3)AttackCenter * GhostKnightAttacker.transform.localScale.x,
            AttackSize,
            0,
            PlayerLayer
        );
        if (PColl != null)
        {
            PColl.GetComponent<IHealth>().Hurt(AttackDamage);
        }

        resetAttack();
    }

    public override void Move()
    {
        if (isAttack || isDie)
            return;

        Collider2D PColl = Physics2D.OverlapBox(
            transform.position + Vector3.up * MoveCenter,
            MoveSize,
            0,
            PlayerLayer
        );
        if (PColl != null)
        {
            Vector3 targetPos = PColl.transform.position - GhostKnightAttacker.transform.position;
            GhostKnightAttacker.transform.localScale = new Vector3(targetPos.x >= 0 ? -1 : 1, 1, 1);
            rigid.linearVelocity = targetPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.softRed * new Color(1, 1, 1, .3f);

        Gizmos.DrawWireCube(transform.position + Vector3.up * MoveCenter, MoveSize);
        Gizmos.DrawWireCube(
            GhostKnightAttacker.transform.position
                + (Vector3)AttackCenter * GhostKnightAttacker.transform.localScale.x,
            AttackSize
        );
    }
}
