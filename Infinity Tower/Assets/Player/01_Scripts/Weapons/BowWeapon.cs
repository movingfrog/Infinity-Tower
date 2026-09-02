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
        arrow.transform.SetParent(null, true);
        arrow.transform.localScale = Vector3.one;
        float finalDamage = AttackDamageCaculator(
            (damage + PlayerStatManager.instance.damage) * (.3f + Percent * .7f)
        );
        Arrow _arrow = arrow.GetComponent<Arrow>();
        fireDirection = (
            (Vector2)transform.parent.position - (Vector2)transform.parent.parent.position
        ).normalized;
        _arrow.Shot(fireDirection, Percent, finalDamage, TriggerAttackEnchant);
        ChargingCoroutine = null;
    }

    IEnumerator Charging()
    {
        float temp = 0;
        arrow = Instantiate(bulletPrefab, shotPosition);
        arrow.transform.localPosition = Vector2.zero;

        Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
        if (arrowRB != null)
            arrowRB.simulated = false;

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
