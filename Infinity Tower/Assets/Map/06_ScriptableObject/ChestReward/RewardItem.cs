using UnityEngine;

[CreateAssetMenu(fileName = "RewardItem", menuName = "Scriptable Objects/Reward/RewardItem")]
public class RewardItem : RewardTable
{
    [Header("아이템 생성 속성")]
    public float commonPercent;
    public float rarePercent;

    [Header("물리 작용")]
    public float xForce;

    public override void ExcuteSpawn(GameObject prefabObject, Transform parent)
    {
        if (prefabObject == null || parent == null)
            return;

        Item item = WorkerHub<ItemCreateWorker>.Instance.CreateItemWorker(
            GameManager.Instance.allEquip,
            commonPercent,
            rarePercent,
            0
        );
        WorkerHub<ItemDropWorker>.Instance.DropItemWork(
            prefabObject,
            item,
            parent.position,
            DropType.Box,
            xForce
        );
    }
}
