using UnityEngine;

public class StatUpgradeNPC : NPC
{
    [Header("스탯 증가 상태")]
    [SerializeField]
    private bool isVola;

    [Header("스탟 증가 재화")]
    [SerializeField]
    private int useAmount;

    public override void OnConfirmAction()
    {
        //플레이어 스탯 증가 함수
        if (isVola) { }
    }

    public override void OnCancelAction() { }
}
