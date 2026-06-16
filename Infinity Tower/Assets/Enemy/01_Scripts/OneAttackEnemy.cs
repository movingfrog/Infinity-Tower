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

    [Header("드랍 아이템"), Space(10f)]
    [SerializeField]
    private Item LootItem;

    [SerializeField]
    private GameObject DroppedLootObject;

    [SerializeField]
    private int minItemCount;

    [SerializeField]
    private int maxItemCount;

    [SerializeField]
    private int minSpawnCount;

    [SerializeField]
    private int maxSpawnCount;

    [SerializeField]
    private float SpawnXForce;

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

    public override void Die()
    {
        base.Die();
        SpawnLoot();
    }

    protected virtual void SpawnLoot()
    {
        if (DroppedLootObject == null)
        {
            Debug.LogError("할당 되지 않았스빈다");
            return;
        }

        int lootCount = UnityEngine.Random.Range(minItemCount, maxItemCount + 1);
        int spawnCount = UnityEngine.Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            WorkerHub<ItemDropWorker>.Instance.DropItemWork(
                DroppedLootObject,
                LootItem,
                transform.position,
                DropType.Inventory,
                SpawnXForce,
                .25f,
                lootCount
            );
        }
    }
}
