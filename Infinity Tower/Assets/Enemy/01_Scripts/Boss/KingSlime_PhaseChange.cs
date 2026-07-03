using UnityEngine;

public partial class KingSlime
{
    [SerializeField]
    private Transform NewPhasePlayerPosition;

    public partial void PhaseChange(Transform PlayerPos)
    {
        WaitNewAction -= .5f;
        PlayerPos.position = NewPhasePlayerPosition.position;
        PatternCoroutine = StartCoroutine(BossActionLoop());
        HP = MaxHP;
        isDie = false;
    }
}
