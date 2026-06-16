using System.Collections;
using UnityEngine;

public class BowWeapon : Weapon
{
    private Coroutine ChargingCoroutine;
    private GameObject arrow;
    private Vector2 fireDirection;

    [Header("활 공격 변수")]
    public Transform shotPosition;
    public GameObject bulletPrefab;
    public bool isCrossBow;
    public LayerMask EnemyLayer;

    private void Start()
    {
        fireDirection = Vector2.right;
    }

    public override void Attack()
    {
        if (endAttack || cooltimeCoroutine != null || ChargingCoroutine != null)
            return;

        ani.SetTrigger("Attack");
        isPushing = true;
        endAttack = true; // 쿨타임 플래그 가동
        ChargingCoroutine = StartCoroutine(Charging());
    }

    public override void EndAttack()
    {
        isPushing = false;
        if (cooltimeCoroutine == null)
            cooltimeCoroutine = StartCoroutine(StartCooltime());
    }

    public override void PositionMove(Vector2 value, float attackRange)
    {
        float ValueX = 1 - Mathf.Abs(GetSign(value.y));
        transform.parent.localPosition = new Vector2(
            attackRange * ValueX,
            attackRange * GetSign(value.y)
        );
        if (value.y == 0)
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.Euler(
                new Vector3(0, 0, 90 * GetSign(value.y) * transform.parent.parent.localScale.x)
            );
        fireDirection = new Vector2(
            ValueX * transform.parent.parent.localScale.x,
            GetSign(value.y)
        );
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
        _arrow.Shot(fireDirection, Percent, finalDamage);
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

        if (arrowRB != null)
            arrowRB.simulated = true;
        ShotArrow(temp);
    }
}
