using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BigFrog : BossSystem
{
    Rigidbody2D rigid;
    bool isAttack;

    [Header("체력 UI변수")]
    [SerializeField]
    private GameObject HealthCanvas;

    private Image HealthBarUI;

    [Header("공용 변수")]
    [SerializeField]
    private LayerMask p_Layer;

    [Header("이동 관련")]
    [SerializeField]
    private float Speed;

    [SerializeField]
    private Vector2 MoveSize;

    [Header("공격 관련")]
    [SerializeField]
    private int Damage;

    [SerializeField, Tooltip("공격 중심 점")]
    private Vector3 AttackPos;

    [SerializeField, Tooltip("공격 범위 크기")]
    private Vector3 AttackSize;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        base.Awake();
    }

    protected override void CreateHPBar()
    {
        GameObject temp = Instantiate(HealthBar, HealthCanvas.transform);
        Image[] allImage = temp.GetComponentsInChildren<Image>();
        foreach (var i in allImage)
        {
            if (i.CompareTag("HealthBar"))
            {
                HealthBarUI = i;
                break;
            }
        }
    }

    protected override void AddPattern()
    {
        patternPool.Add(Attacking);
    }

    private IEnumerator Attacking()
    {
        Collider2D p_Coll = Physics2D.OverlapBox(transform.position, MoveSize, 0, p_Layer);
        if (p_Coll != null)
        {
            while (p_Coll != null)
            {
                Vector3 pos = p_Coll.transform.position;
                float direction = Mathf.Sign(pos.x - transform.position.x);

                var scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * direction;
                transform.localScale = scale;

                Collider2D t_Coll = Physics2D.OverlapBox(
                    transform.position + (Vector3.right * AttackPos.x * direction),
                    AttackSize,
                    0,
                    p_Layer
                );
                if (t_Coll != null)
                {
                    ani.SetTrigger("isAttack");
                    isAttack = true;
                    break;
                }

                ani.SetBool("isRun", true);
                rigid.linearVelocityX = direction * Speed;

                yield return null;
                p_Coll = Physics2D.OverlapBox(transform.position, MoveSize, 0, p_Layer);
            }

            ani.SetBool("isRun", false);
            rigid.linearVelocityX = 0;
        }
        else
        {
            yield return StartCoroutine(Groggy());
        }
        yield return new WaitUntil(() => !isAttack);
    }

    public void Attack()
    {
        Collider2D target = Physics2D.OverlapBox(
            transform.position + (Vector3.right * AttackPos.x * Mathf.Sign(transform.localScale.x)),
            AttackSize,
            0,
            p_Layer
        );
        if (target != null && target.TryGetComponent(out IHealth p_Health))
        {
            p_Health.Hurt(Damage, gameObject);
            target.GetComponent<PlayerController>().StartCoroutine(SlowPlayer());
            ani.SetBool("isSucc", true);
        }
    }

    public void SwallowHeal()
    {
        Heal(MaxHP / 10f, null);
        isAttack = false;
        ani.SetBool("isSucc", false);
    }

    public override void Hurt(float damage, GameObject attacker)
    {
        if (HP - damage > 0)
        {
            HP -= damage;
            ShowDamage(damage, Color.white);
            HealthBarUI.fillAmount = HP / MaxHP;
            _damageFlash.CallDamageFlash();
        }
        else
        {
            HP -= HP;
            ShowDamage(damage, Color.white);
            HealthBarUI.fillAmount = 0;
            _damageFlash.CallDamageFlash();
            Die();
        }
    }

    protected override IEnumerator Groggy()
    {
        yield return new WaitForSeconds(GroggyTime);
    }

    private IEnumerator SlowPlayer()
    {
        PlayerStatManager.instance.statUp(StatType.SPEED, -15);

        yield return new WaitForSeconds(3f);

        PlayerStatManager.instance.statUp(StatType.SPEED, 15);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.darkCyan * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireCube(transform.position, MoveSize);
        Gizmos.DrawWireCube(
            transform.position + (Vector3.right * AttackPos.x * Mathf.Sign(transform.localScale.x)),
            AttackSize
        );
    }
}
