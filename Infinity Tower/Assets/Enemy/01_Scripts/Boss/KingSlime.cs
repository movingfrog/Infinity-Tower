using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KingSlimePatternState
{
    Bubble,
    Rain,
    Jump,
    Melt,
    Bite,
}

public partial class KingSlime : BossSystem
{
    [Header("체력 관련")]
    private int HealthCount = 4;
    private List<Image> AllHealth;
    private List<Image> HealthImage;

    [Header("공격 관련")]
    [Tooltip("패턴 범위 확인")]
    [SerializeField]
    private KingSlimePatternState state;

    [SerializeField]
    private Vector2[] AttackAreaValue;

    [SerializeField]
    private GameObject[] AttackArea;

    [SerializeField]
    private GameObject[] AttackPattern;

    [SerializeField]
    private LayerMask PlayerLayer;

    [Header("패턴 시간")]
    [SerializeField]
    private float[] ChargeTime;

    [SerializeField]
    private float RainPatternTime;

    [SerializeField]
    private float BubblePatternTime;

    [SerializeField]
    private float AdditionalWaitTime;

    [Header("공격 값")]
    [SerializeField]
    private float BubbleDamage;

    [SerializeField]
    private int maxBubbleAttackCount = 3;

    [Space(10f), SerializeField]
    private float PoisionDamage;

    [SerializeField]
    private float PosionTickRate;

    [SerializeField]
    private int PoisionTickAmount;

    [Space(10f), SerializeField]
    private float JumpForce = 15;

    [SerializeField]
    private float JumpkDamage;

    [Space(10f), SerializeField]
    private float BiteDamage;

    [Space(10f), SerializeField]
    private float HealTickRate;

    [SerializeField]
    private int HealAmount;

    [SerializeField]
    private int MaxHealAmount;

    [Header("페이즈 패턴")]
    [SerializeField]
    private Transform[] PhasePos;

    [SerializeField]
    private GameObject Guide;

    private Rigidbody2D rigid;

    private Coroutine PatternCoroutine;

    private const float centerToGroundDistance = .3f;

    private const int maxBubbleAmount = 40;
    private const float bubbleRadius = .6f;

    protected override void Awake()
    {
        base.Awake();
        TryGetComponent(out rigid);
    }

    protected override void Start()
    {
        PatternCoroutine = StartCoroutine(BossActionLoop());
    }

    protected override void CreateHPBar()
    {
        GameObject temp = Instantiate(HealthBar, parentCanvas.transform);
        temp.GetComponentsInChildren<Image>(true, AllHealth);
        foreach (var image in AllHealth)
        {
            if (image.CompareTag("HealthBar"))
            {
                HealthImage.Add(image);
            }
        }
    }

    public override void Hurt(float damage)
    {
        if (HP - damage > 0)
        {
            HP -= damage;
            HealthImage[HealthCount].fillAmount = HP / MaxHP;
            ShowDamage(damage, Color.white);
            _damageFlash.CallDamageFlash();
        }
        else
        {
            HP -= HP;
            HealthImage[HealthCount].fillAmount = HP / MaxHP;
            ShowDamage(damage, Color.white);
            _damageFlash.CallDamageFlash();
            Guide.SetActive(true);
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDie && collision.gameObject.CompareTag("Player"))
        {
            Guide.SetActive(false);
            collision.transform.position = PhasePos[HealthCount].position;
        }
    }

    public partial void PhaseChange(Transform PlayerPos);

    public override void Die()
    {
        if (!isDie)
        {
            StopCoroutine(PatternCoroutine);
            if (HealthCount > 0)
            {
                HealthCount--;
                isDie = true;
            }
            else
            {
                Debug.Log("죽고 보상을 주는 코드");
            }
        }
    }

    protected override void AddPattern()
    {
        patternPool.Add(BubbleAttack);
        patternPool.Add(PoisonRain);
        patternPool.Add(JumpAttack);
        patternPool.Add(BiteAttack);
        patternPool.Add(MeltHeal);
    }

    #region 보스 패턴
    private partial IEnumerator BubbleAttack();

    private partial IEnumerator PoisonRain();

    private partial IEnumerator JumpAttack();

    private partial IEnumerator MeltHeal();

    private partial IEnumerator BiteAttack();

    protected override IEnumerator Groggy()
    {
        yield return new WaitForSeconds(GroggyTime);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1, 1, 1, .3f);
        switch (state)
        {
            case KingSlimePatternState.Bubble:
                Gizmos.DrawWireCube(
                    transform.position
                        + Vector3.down * transform.localScale.y * centerToGroundDistance,
                    AttackAreaValue[0]
                );
                break;
            case KingSlimePatternState.Rain:
                Gizmos.DrawWireCube(transform.position, AttackAreaValue[1]);
                break;
            case KingSlimePatternState.Jump:
                Gizmos.DrawWireCube(
                    transform.position
                        + Vector3.down * centerToGroundDistance * transform.localScale.y,
                    AttackAreaValue[2]
                );
                break;
            case KingSlimePatternState.Bite:
                Gizmos.DrawWireCube(
                    transform.position
                        + Vector3.right
                            * transform.localScale.x
                            * (
                                AttackArea[3].transform.localPosition.x
                                + AttackAreaValue[3].x / (2 * Mathf.Abs(transform.localScale.x))
                            ),
                    AttackAreaValue[3]
                );
                break;
        }
    }
}
