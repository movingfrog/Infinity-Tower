using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_ThornmailAbility",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_ThornmailAbility"
)]
public class SO_ThornmailAbility : SO_AbilityTechData, IOnHit
{
    [SerializeField]
    private int[] d_Percent = { 0, 5, 5, 10 };

    [SerializeField]
    private int[] Percent = { 0, 10, 30, 30 };

    public override string GetEffect(int level)
    {
        int c = level - 1;
        int f = level;
        return string.Format(_effect, Percent[c], Percent[f], d_Percent[c], d_Percent[f]);
    }

    public override string GetExplane(int level)
    {
        int f = level;
        return string.Format(_explane, Percent[f], d_Percent[f]);
    }

    public void OnHit(int level, ref HitContext ctx)
    {
        if (Random.Range(0, 100) < Percent[level])
        {
            if (ctx.AttackEnemy.TryGetComponent(out IHealth target))
            {
                target.Hurt(d_Percent[level], ctx.AttackEnemy);
            }
        }
    }
}
