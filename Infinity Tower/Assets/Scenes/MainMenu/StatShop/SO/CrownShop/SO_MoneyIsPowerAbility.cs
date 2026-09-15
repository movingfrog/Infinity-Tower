using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_MoneyIsPowerAbility",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_MoneyIsPowerAbility"
)]
public class SO_MoneyIsPowerAbility : SO_AbilityTechData
{
    private readonly int u_Percent = 10;
    private int stack = 0;

    [SerializeField]
    private int m_Amount = 1000;

    public override string GetExplane(int level)
    {
        return string.Format(_explane, m_Amount, u_Percent, 10);
    }

    public void Commit(int MoneyAmount)
    {
        if (stack < 10)
        {
            int f_stack = stack;
            stack = MoneyAmount / m_Amount;
            stack = Mathf.Min(stack, 10);
            int p_amount = stack - f_stack;
            var r = new TechStatModifier(
                StatType.ATK,
                ModifierType.PercentAdd,
                p_amount * u_Percent
            );
            PlayerStatManager.instance.ApplyToStat(r);
        }
    }
}
