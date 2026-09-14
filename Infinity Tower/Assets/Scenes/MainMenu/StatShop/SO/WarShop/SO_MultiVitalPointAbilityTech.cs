using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_MultiVitalPoint",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_MultiVitalPoint"
)]
public class SO_MultiVitalPointAbilityTech : SO_AbilityTechData, IOnAttack
{
    private int currentCritAmount;

    private int maxCritAmount(int level) => level < 2 ? 5 : 10;

    [SerializeField]
    private float d_Percent;

    private float GetPercent(int level) =>
        level switch
        {
            1 => -10,
            2 => -30,
            3 => -15,
            _ => 0,
        };

    public override string GetEffect(int level)
    {
        if (level <= 0)
            Debug.LogError("값이 정상적이지 않습니다");

        return string.Format(
            _effect,
            maxCritAmount(level - 1),
            maxCritAmount(level),
            GetPercent(level - 1),
            GetPercent(level)
        );
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, maxCritAmount(level), GetPercent(level));
    }

    public override void Apply(int level)
    {
        TechStatModifier S = new TechStatModifier(
            StatType.CRIT_RATE,
            ModifierType.PercentAdd,
            GetPercent(level)
        );
        PlayerStatManager.instance.ApplyToStat(S);
        currentCritAmount = 0;
        base.Apply(level);
    }

    public void OnAttack(int level, ref AttackContext ctx)
    {
        if (ctx.isCrit)
        {
            if (currentCritAmount < maxCritAmount(level))
            {
                TechStatModifier runtimeS = new TechStatModifier(
                    StatType.CRIT_DMG,
                    ModifierType.PercentAdd,
                    10
                );
                PlayerStatManager.instance.ApplyToStat(runtimeS);
                currentCritAmount++;
            }
        }
        else
        {
            TechStatModifier runtimeS = new TechStatModifier(
                StatType.CRIT_DMG,
                ModifierType.PercentAdd,
                -10 * currentCritAmount
            );
            PlayerStatManager.instance.ApplyToStat(runtimeS);
            currentCritAmount = 0;
        }
    }
}
