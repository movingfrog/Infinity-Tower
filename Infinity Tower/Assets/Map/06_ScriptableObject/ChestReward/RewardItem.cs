using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardItem", menuName = "Scriptable Objects/Reward/RewardItem")]
public class RewardItem : RewardTable
{
    [Header("아이템 생성 속성")]
    public ItemLevel level { get; set; }
    public List<Item> items { get; set; }
    public float commonPercent { get; set; }
    public float rarePercent { get; set; }

    [Header("물리 작용")]
    public float xForce;

    public override void ExcuteSpawn(GameObject prefabObject, Transform parent)
    {
        if (prefabObject == null || parent == null)
            return;

        Item item = WorkerHub<ItemCreateWorker>.Instance.CreateItemWorker(
            items,
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
