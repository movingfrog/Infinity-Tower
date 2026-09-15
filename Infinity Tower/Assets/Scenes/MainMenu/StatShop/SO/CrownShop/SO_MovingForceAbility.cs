using System.Collections;
using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_MovingForceAbility",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_MovingForceAbility"
)]
public class SO_MovingForceAbility : SO_AbilityTechData
{
    private readonly int u_Percent = 10;
    private int stack = 0;
    private int force = 0;
    private Coroutine currentCoroutine;

    [SerializeField]
    private int MaxStack = 10;
    WaitForSeconds s = new WaitForSeconds(1f);

    public override string GetExplane(int level)
    {
        return string.Format(_explane, u_Percent, MaxStack);
    }

    public void Commit(int Force, MonoBehaviour P)
    {
        force = Force;
        if (currentCoroutine == null)
        {
            currentCoroutine = P.StartCoroutine(ChangeStack());
        }
    }

    IEnumerator ChangeStack()
    {
        while (true)
        {
            int f_Stack = stack;
            stack += force != 0 ? 1 : -1;
            stack = Mathf.Min(stack, MaxStack);
            var r = new TechStatModifier(
                StatType.ATK,
                ModifierType.PercentAdd,
                (stack - f_Stack) * u_Percent
            );
            PlayerStatManager.instance.ApplyToStat(r);
            yield return s;
        }
    }
}
