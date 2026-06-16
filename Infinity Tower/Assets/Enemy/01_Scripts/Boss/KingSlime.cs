using System.Collections;
using UnityEngine;

public class KingSlime : BossSystem
{
    protected override void AddPattern()
    {
        patternPool.Add(BubbleAttack);
        patternPool.Add(PoisonRain);
        patternPool.Add(JumpAttack);
        patternPool.Add(BiteAttack);
        patternPool.Add(MeltHeal);
    }

    #region 보스 패턴
    IEnumerator BubbleAttack()
    {
        yield return null;
    }

    IEnumerator PoisonRain()
    {
        yield return null;
    }

    IEnumerator JumpAttack()
    {
        yield return null;
    }

    IEnumerator MeltHeal()
    {
        yield return null;
    }

    IEnumerator BiteAttack()
    {
        yield return null;
    }
    #endregion
}
