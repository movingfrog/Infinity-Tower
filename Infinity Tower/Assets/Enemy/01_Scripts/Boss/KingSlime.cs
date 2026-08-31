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
    [Header("죽음 패턴 관련")]
    [SerializeField]
    private Vector2 BlackHoleSize;

    [SerializeField]
    private float BlackHoleForce;

    [Header("체력 관련")]
    [SerializeField]
    private GameObject HealthBarCanvas;

    private int HealthCount = 5;
    private List<Image> AllHealth = new List<Image>();
    private List<Image> HealthImage = new List<Image>();

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

    private const int maxBubbleAmount = 10;
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
        GameObject temp = Instantiate(HealthBar, HealthBarCanvas.transform);
        temp.GetComponentsInChildren<Image>(true, AllHealth);
        foreach (var image in AllHealth)
        {
            if (image.CompareTag("HealthBar"))
            {
                HealthImage.Add(image);
            }
        }
        Debug.Log(HealthImage.Count);
    }

    public override void Hurt(float damage)
    {
        if (HP - damage > 0)
        {
            HP -= damage;
            HealthImage[HealthCount - 1].fillAmount = HP / MaxHP;
            ShowDamage(damage, Color.white);
            _damageFlash.CallDamageFlash();
        }
        else
        {
            HP -= HP;
            HealthImage[HealthCount - 1].fillAmount = HP / MaxHP;
            ShowDamage(damage, Color.white);
            _damageFlash.CallDamageFlash();
            Guide.SetActive(true);
            Die();
        }
    }

    private void FixedUpdate()
    {
        Collider2D PColl = Physics2D.OverlapBox(transform.position, BlackHoleSize, 0, PlayerLayer);
        if (PColl != null)
        {
            float ScaleX =
                Mathf.Abs(transform.localScale.x)
                * ((PColl.transform.position.x - transform.position.x) > 0 ? 1 : -1);
            transform.localScale = new Vector3(
                ScaleX,
                transform.localScale.y,
                transform.localScale.z
            );
        }
        if (isDie)
        {
            if (PColl != null)
            {
                PColl.transform.position = Vector3.Lerp(
                    PColl.transform.position,
                    transform.position,
                    BlackHoleForce * Time.deltaTime
                );
                if ((PColl.transform.position - transform.position).magnitude < .1f)
                {
                    Guide.SetActive(false);
                    PColl.transform.position = PhasePos[HealthCount - 1].position;
                }
            }
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
                if (HealthCount == 0)
                {
                    HealthImage[HealthCount].gameObject.SetActive(true);
                }
                isDie = true;
            }
            else
            {
                Destroy(gameObject);
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
        Gizmos.DrawWireCube(transform.position, BlackHoleSize);
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
