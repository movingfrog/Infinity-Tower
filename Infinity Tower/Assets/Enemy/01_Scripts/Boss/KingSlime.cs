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

public class KingSlime : BossSystem
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
    IEnumerator BubbleAttack()
    {
        GameObject useArea = AttackArea[0];
        float areaSize = useArea.transform.localScale.x;
        float f = 0;
        useArea.SetActive(true);
        while (f < ChargeTime[0])
        {
            f += Time.deltaTime;
            float sizeX = Mathf.Lerp(0, areaSize, f / ChargeTime[0]);
            useArea.transform.localScale = new Vector3(
                sizeX,
                useArea.transform.localScale.y,
                useArea.transform.localScale.z
            );
            yield return null;
        }
        useArea.SetActive(false);
        int randAttackCount = Random.Range(2, maxBubbleAttackCount);
        for (int i = 1; i <= randAttackCount; i++)
        {
            for (int j = 0; j < maxBubbleAmount; j++)
            {
                GameObject bubble = Instantiate(AttackPattern[0], transform);
                Vector3 BS = bubble.transform.localScale;
                Vector3 S = transform.localScale;
                bubble.transform.localScale = new Vector3(BS.x / S.x, BS.y / S.y, BS.z / S.z);
                if (j < maxBubbleAmount / 2)
                {
                    bubble.transform.localPosition = new Vector2(
                        -bubbleRadius * (i % 2) + j * bubbleRadius * 2,
                        -centerToGroundDistance
                    ); //y값은 킹슬라임에서 바닥까지 떨어진 길이
                }
                else
                {
                    bubble.transform.localPosition = new Vector2(
                        bubbleRadius * (i % 2) + (j - maxBubbleAmount / 2) * bubbleRadius * 2,
                        -centerToGroundDistance
                    );
                }
            }
            yield return new WaitForSeconds(BubblePatternTime / randAttackCount);
        }
    }

    IEnumerator PoisionDotDamage(Collider2D Player)
    {
        IHealth PlayerHealth = Player.GetComponent<IHealth>();
        for (int i = 0; i < PoisionTickAmount; i++)
        {
            PlayerHealth.Hurt(PoisionDamage);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PoisonRain()
    {
        float f = 0;
        GameObject useArea = AttackArea[1];
        float areaSize = useArea.transform.localScale.x;
        useArea.SetActive(true);
        while (f < ChargeTime[1])
        {
            f += Time.deltaTime;
            float sizeX = Mathf.Lerp(0, areaSize, f / ChargeTime[1]);
            useArea.transform.localScale = new Vector3(
                sizeX,
                useArea.transform.localScale.y,
                useArea.transform.localScale.z
            );
            yield return null;
        }
        useArea.SetActive(false);
        Collider2D playerCollider = Physics2D.OverlapBox(
            transform.position,
            AttackAreaValue[1],
            0,
            PlayerLayer
        );
        if (playerCollider != null)
        {
            GameObject rainPattern = Instantiate(AttackPattern[1]);
            ParticleSystem rainParticle = rainPattern.GetComponentInChildren<ParticleSystem>();
            rainPattern.transform.position = playerCollider.transform.position;
            yield return new WaitForSeconds(AdditionalWaitTime);
            f = 0;
            while (f < RainPatternTime)
            {
                f += Time.deltaTime;
                Collider2D playerColliderInRain = Physics2D.OverlapBox(
                    rainPattern.transform.position,
                    new Vector2(rainParticle.shape.radius * 2, 5),
                    PlayerLayer
                );
                if (playerColliderInRain != null)
                {
                    StartCoroutine(PoisionDotDamage(playerColliderInRain));
                    break;
                }
                yield return null;
            }
        }
        else
            yield return StartCoroutine(Groggy());
    }

    IEnumerator JumpAttack()
    {
        ani.SetBool("Charge", true);
        yield return new WaitForSeconds(ChargeTime[2]);
        ani.SetBool("Charge", false);
        float areaPos = AttackArea[2].transform.position.y;
        rigid.AddForceY(JumpForce, ForceMode2D.Impulse);
        yield return new WaitUntil(() => rigid.linearVelocityY == 0);
        rigid.gravityScale = 0;
        GameObject useArea = AttackArea[2];
        float areaSize = useArea.transform.localScale.x;
        float t = 0;
        useArea.SetActive(true);
        while (t < AdditionalWaitTime)
        {
            t += Time.deltaTime;
            useArea.transform.position = areaPos * Vector2.up;
            float sizeX = Mathf.Lerp(0, areaSize, t / AdditionalWaitTime);
            yield return null;
        }
        useArea.SetActive(false);
        rigid.gravityScale = 1;
        yield return new WaitUntil(() => rigid.linearVelocityY == 0);
        Collider2D Player = Physics2D.OverlapBox(
            transform.position + Vector3.down * centerToGroundDistance * transform.localScale.y,
            AttackAreaValue[2],
            0,
            PlayerLayer
        );
        if (Player != null)
        {
            float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);
            float attackRange = AttackAreaValue[2].x / 2;
            IHealth PHP = Player.GetComponent<IHealth>();
            if (attackRange * .3f >= distance)
            {
                PHP.Hurt(JumpkDamage);
                Debug.Log("점프시키는 기능 추가할 수 있음");
            }
            else if (attackRange * .7f >= distance)
            {
                PHP.Hurt(JumpkDamage * .5f);
                Debug.Log("점프시키는 기능 추가할 수 있음");
            }
            else
            {
                PHP.Hurt(JumpkDamage * .3f);
                Debug.Log("점프시키는 기능 추가할 수 있음");
            }
        }
        Debug.Log("먼지 이펙트 필요");
        yield return StartCoroutine(Groggy());
    }

    IEnumerator MeltHeal()
    {
        ani.SetBool("Charge", true);
        yield return new WaitForSeconds(ChargeTime[4]);
        ani.SetBool("Charge", false);
        for (int i = 0; i < HealAmount; i++)
        {
            Heal(MaxHealAmount / HealAmount, null);
            yield return new WaitForSeconds(HealTickRate);
        }
        yield return StartCoroutine(Groggy());
    }

    IEnumerator BiteAttack()
    {
        yield return null;
        GameObject useArea = AttackArea[3];
        float areaSize = useArea.transform.localScale.x;
        float t = 0;
        useArea.SetActive(true);
        while (t < ChargeTime[3])
        {
            t += Time.deltaTime;
            float sizeX = Mathf.Lerp(0, areaSize, t / ChargeTime[3]);
            useArea.transform.localScale = new Vector3(
                sizeX,
                useArea.transform.localScale.y,
                useArea.transform.localScale.z
            );
            yield return null;
        }
        yield return new WaitForSeconds(AdditionalWaitTime);
        Collider2D Player = Physics2D.OverlapBox(
            transform.position
                + Vector3.right
                    * transform.localScale.x
                    * (
                        AttackArea[3].transform.localPosition.x
                        + AttackAreaValue[3].x / (2 * Mathf.Abs(transform.localScale.x))
                    ),
            AttackAreaValue[3],
            0,
            PlayerLayer
        );
        if (Player != null)
        {
            Player.GetComponent<IHealth>().Hurt(BiteDamage);
        }
        else
            yield return StartCoroutine(Groggy());
    }

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
