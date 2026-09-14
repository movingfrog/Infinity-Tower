using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_FeelLikeFullAbility",
    menuName = "Scriptable Objects/TechDataSO/SO_Ability/SO_FeelLikeFullAbility"
)]
public class SO_FeelLikeFullAbility : SO_AbilityTechData
{
    [SerializeField]
    private int IncreaseAmount;

    public override string GetEffect(int level)
    {
        return string.Format(_effect, IncreaseAmount * level);
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, IncreaseAmount * level);
    }

    public void Commit(int level, int u_Health)
    {
        int r = u_Health * IncreaseAmount * level;
        TechStatModifier r_stat = new TechStatModifier(StatType.ATK, ModifierType.PercentAdd, r);
        PlayerStatManager.instance.ApplyToStat(r_stat);
    }
}
