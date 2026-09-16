using System.Collections;
using TreeEditor;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class OldKinght : BossSystem
{
    Rigidbody2D rigid;
    bool isAttacking;

    [Header("이동 변수")]
    [SerializeField]
    private float Speed = 1;

    [Header("체력 UI변수")]
    [SerializeField]
    private GameObject HealthbarCanvas;

    [Tooltip("체력 바 변수")]
    private Image HealthBarUI;

    [SerializeField, Tooltip("차징 시 변경 될 색")]
    private Color HealthColor = Color.red;

    [Header("공격 변수")]
    [SerializeField, Tooltip("공격 범위")]
    private Vector2 attackSize;

    [SerializeField, Tooltip("공격 범위 중심")]
    private Vector2 attackPos;

    [SerializeField, Tooltip("최초 공격 데미지")]
    private int Damage;
    private int f_Damage;

    [SerializeField, Tooltip("데미지 증가 값")]
    private int u_Damage = 1;

    [Header("공통 변수")]
    [SerializeField, Tooltip("플레이어 Layer")]
    private LayerMask p_Layer;

    [Header("플레이어 범위")]
    [SerializeField, Tooltip("플레이어를 찾을 범위 지정")]
    private float Radius;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        f_Damage = Damage;
        base.Awake();
    }

    protected override void AddPattern()
    {
        patternPool.Add(Attacking);
    }

    protected override void CreateHPBar()
    {
        GameObject temp = Instantiate(HealthBar, HealthbarCanvas.transform);
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

    private IEnumerator Attacking()
    {
        Collider2D p_Coll = Physics2D.OverlapCircle(transform.position, Radius, p_Layer);
        if (p_Coll != null)
        {
            float threshold = Mathf.Max(.1f, Speed * Time.fixedDeltaTime * 1.5f);
            while (p_Coll != null)
            {
                Vector3 pos = p_Coll.transform.position;
                float direction = Mathf.Sign(pos.x - transform.position.x);

                var scale = transform.localScale;
                scale.x = direction;
                transform.localScale = scale;

                Collider2D targetColl = Physics2D.OverlapBox(
                    transform.position + (Vector3)(attackPos * Vector3.right * direction),
                    attackSize,
                    0,
                    p_Layer
                );

                if (targetColl != null)
                {
                    ani.SetTrigger("isAttack");
                    isAttacking = true;
                    break;
                }

                if (Mathf.Abs(pos.x - transform.position.x) <= threshold)
                {
                    break;
                }

                ani.SetBool("isRun", true);
                rigid.linearVelocityX = direction * Speed;

                yield return null;
                p_Coll = Physics2D.OverlapCircle(transform.position, Radius, p_Layer);
            }

            rigid.linearVelocityX = 0;
            ani.SetBool("isRun", false);
        }
        else
        {
            yield return StartCoroutine(Groggy());
        }
        yield return new WaitUntil(() => !isAttacking);
    }

    public void Attack()
    {
        Collider2D targetColl = Physics2D.OverlapBox(
            transform.position
                + (Vector3)(attackPos * Vector3.right * Mathf.Sign(transform.localScale.x)),
            attackSize,
            0,
            p_Layer
        );
        if (targetColl != null)
        {
            IHealth p_Health = targetColl.GetComponent<IHealth>();
            p_Health.Hurt(Damage, gameObject);
            ani.SetBool("isCharge", true);
        }
        else
            isAttacking = false;
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

    public void Charging()
    {
        Damage += u_Damage;
        isAttacking = false;
        float damagePercent = (Damage - f_Damage) / 10f;
        HealthBarUI.color = Color.Lerp(HealthBarUI.color, HealthColor, damagePercent);
        ani.SetBool("isCharge", false);
    }

    protected override IEnumerator Groggy()
    {
        yield return new WaitForSeconds(GroggyTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.violetRed * new Color(1, 1, 1, .3f);

        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.DrawWireCube(
            transform.position + (Vector3)(attackPos * Vector3.right * transform.localScale.x),
            attackSize
        );
    }
}
