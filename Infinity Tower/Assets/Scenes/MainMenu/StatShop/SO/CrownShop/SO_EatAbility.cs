using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_EatAbility",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_EatAbility"
)]
public class SO_EatAbility : SO_AbilityTechData, IOnAttack
{
    [SerializeField]
    private int HealPercent = 10;

    [SerializeField]
    private int HealProb = 30;

    public override string GetExplane(int level)
    {
        return string.Format(_explane, HealProb, HealPercent);
    }

    public void OnAttack(int level, ref AttackContext ctx)
    {
        if (ctx.TargetEnemy == null)
        {
            if (Random.Range(0, 100) < HealProb)
            {
                PlayerStatManager.instance.ChangeHealth(
                    PlayerStatManager.instance.MaxHP * HealPercent / 100f
                );
            }
        }
    }
}
