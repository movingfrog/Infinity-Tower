using UnityEngine;

public struct AttackContext
{
    public IHealth TargetEnemy;
    public float FinalDamage;
    public bool isCrit;

    public AttackContext(IHealth target, float damage, bool critstate)
    {
        TargetEnemy = target;
        FinalDamage = damage;
        isCrit = critstate;
    }
}

public struct HitContext
{
    public GameObject AttackEnemy;
    public readonly float OriginDamage;
    public float FinalDamage;

    public HitContext(float damage, GameObject attackEnemy)
    {
        AttackEnemy = attackEnemy;
        OriginDamage = damage;
        FinalDamage = damage;
    }
}

public interface IOnAcquire
{
    void OnAcquire(int level);
}

public interface IOnAttack
{
    void OnAttack(int level, ref AttackContext ctx);
}

public interface IOnHit
{
    void OnHit(int level, ref HitContext ctx);
}
