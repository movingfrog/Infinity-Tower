using System.Collections;
using System.Collections.Generic;

public class BossSystem : parentEnemy
{
    private List<System.Func<IEnumerator>> patternPool = new List<System.Func<IEnumerator>>();
}
