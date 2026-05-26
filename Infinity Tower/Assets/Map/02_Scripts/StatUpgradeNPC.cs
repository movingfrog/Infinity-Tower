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
        if (isVola)
        {
            if (InventoryManager.Instance.UseGoods(GoodsType.Stone, (uint)useAmount) || true)
            {
                StatType randStat = GetRandomEnumValue<StatType>();
                while (randStat == StatType.CRIT_RATE)
                    randStat = GetRandomEnumValue<StatType>();
                int randValue = Random.Range(1, 6);
                PlayerStatManager.instance.statUp(randStat, randValue);
                Debug.Log(randStat.ToString());
                Debug.Log(randValue);
            }
        }
    }

    public override void OnCancelAction() { }

    private T GetRandomEnumValue<T>()
    {
        var enumValue = System.Enum.GetValues(typeof(T));
        return (T)enumValue.GetValue(Random.Range(0, enumValue.Length));
    }
}
