using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BowWeapon : Weapon
{
    private Coroutine ChargingCoroutine;
    private GameObject arrow;
    private Vector2 fireDirection;
    private float coolTimeRate;

    [Header("활 공격 변수")]
    public Transform shotPosition;
    public GameObject bulletPrefab;
    public bool isCrossBow;
    public LayerMask EnemyLayer;

    protected override void Start()
    {
        base.Start();
        fireDirection = Vector2.right;
        coolTimeRate = attackRate;
    }

    public override void Attack()
    {
        if (cooltimeCoroutine != null || ChargingCoroutine != null)
            return;

        TriggerHitEnchants();
        ani.SetTrigger("Attack");
        isPushing = true;
        ChargingCoroutine = StartCoroutine(Charging());
    }

    public override void EndAttack()
    {
        isPushing = false;
    }

    private void ShotArrow(float Percent)
    {
        ani.SetTrigger("Shot");
        if (!isCrossBow)
            WorkerHub<SoundWorker>.Instance.PlaySFX(
                GameManager.Instance.Source,
                GameManager.Instance.SFX.GetClip(SoundType.BowAttack)
            );
        arrow.transform.SetParent(null, true);
        arrow.transform.localScale = Vector3.one;
        float _damage =
            (damage + PlayerStatManager.instance.Atk)
            * PlayerStatManager.instance.damage
            * (.3f + Percent * .7f);
        float finalDamage = AttackDamageCaculator(_damage);
        Arrow _arrow = arrow.GetComponent<Arrow>();
        fireDirection = (
            (Vector2)transform.parent.position - (Vector2)transform.parent.parent.position
        ).normalized;
        _arrow.Shot(fireDirection, Percent, finalDamage, _damage, TriggerAttackEnchant);
        ChargingCoroutine = null;
    }

    protected override IEnumerator StartCooltime()
    {
        yield return new WaitForSeconds(attackRate);
        cooltimeCoroutine = null;
        if (isCrossBow)
            WorkerHub<SoundWorker>.Instance.PlaySFX(
                GameManager.Instance.Source,
                GameManager.Instance.SFX.GetClip(SoundType.BowCharge)
            );
    }

    IEnumerator Charging()
    {
        float temp = 0;
        arrow = Instantiate(bulletPrefab, shotPosition);
        arrow.transform.localPosition = Vector2.zero;

        Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
        if (arrowRB != null)
            arrowRB.simulated = false;

        if (isCrossBow)
            WorkerHub<SoundWorker>.Instance.PlaySFX(
                GameManager.Instance.Source,
                GameManager.Instance.SFX.GetClip(SoundType.BowAttack)
            );
        while (isPushing)
        {
            if (isCrossBow)
            {
                temp = 1;
                break;
            }
            temp = Mathf.Min(temp + Time.deltaTime, 1f);
            yield return null;
        }
        attackRate = coolTimeRate * (.3f + temp * .7f);
        cooltimeCoroutine = StartCoroutine(StartCooltime());
        if (arrowRB != null)
            arrowRB.simulated = true;
        ShotArrow(temp);
    }
}
