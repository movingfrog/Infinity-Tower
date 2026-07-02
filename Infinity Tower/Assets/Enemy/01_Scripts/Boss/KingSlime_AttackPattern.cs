using System.Collections;
using UnityEngine;

public partial class KingSlime
{
    private partial IEnumerator BubbleAttack()
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
                bubble.transform.parent = null;
            }
            yield return new WaitForSeconds(BubblePatternTime / randAttackCount);
        }
    }
}

public partial class KingSlime
{
    private partial IEnumerator PoisonRain()
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

    IEnumerator PoisionDotDamage(Collider2D Player)
    {
        IHealth PlayerHealth = Player.GetComponent<IHealth>();
        for (int i = 0; i < PoisionTickAmount; i++)
        {
            PlayerHealth.Hurt(PoisionDamage);
            yield return new WaitForSeconds(1f);
        }
    }
}

public partial class KingSlime
{
    private partial IEnumerator JumpAttack()
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
        yield return new WaitUntil(() => rigid.linearVelocityY < 0.01f);
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
}

public partial class KingSlime
{
    private partial IEnumerator MeltHeal()
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
}

public partial class KingSlime
{
    private partial IEnumerator BiteAttack()
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
}
