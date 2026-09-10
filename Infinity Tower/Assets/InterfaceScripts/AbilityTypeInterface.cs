using UnityEngine;

public struct AttackContext
{
    public IHealth TargetEnemy;
    public float FinalDamage;
    public bool isCrit;
}

public struct HitContext
{
    public IHealth AttackEnemy;
    public float FinalDamage;
}

public interface IOnAcquire
{
    void OnAcquire(int level);
}

public interface IOnAttack
{
    void OnAttack(int level, AttackContext ctx);
}

public interface IOnHit
{
    void OnHit(int level, HitContext ctx);
}
