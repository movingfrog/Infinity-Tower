using System;
using System.Collections;
using UnityEngine;

public abstract class OneAttackEnemy : parentEnemy, IAttack, IMove
{
    public bool isAttack { get; set; }
    public Rigidbody2D rigid { get; set; }

    [field: SerializeField]
    public float AttackDamage { get; set; }

    [field: SerializeField]
    [Range(0f, 1f)]
    public float attackDelay { get; set; }

    [field: SerializeField]
    public float Speed { get; set; }

    protected override void Awake()
    {
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (TimeManager.Instance.isRewinding)
            return;

        Attack();
        Move();
    }

    public void resetAttack() =>
        StartCoroutine(waitAttackCool(attackDelay, () => isAttack = false));

    public abstract void Attack();

    public abstract void Move();

    public IEnumerator waitAttackCool(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
