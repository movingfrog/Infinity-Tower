using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public interface IWorker { }

public static class WorkerHub<T>
    where T : struct, IWorker
{
    public static readonly T Instance = new T();
}

public struct CameraMoveWorker : IWorker
{
    public void confinerChange(Collider2D polygon, CinemachineConfiner2D confiner) =>
        confiner.BoundingShape2D = polygon;
}

public enum DropType
{
    Inventory,
    Shop,
    Box,
}

public struct ItemDropWorker : IWorker
{
    public void DropItemWork(
        GameObject prefab,
        Item data,
        Vector3 pos,
        DropType type,
        float Xforce,
        float Yforce = .25f,
        int itemCount = 0,
        GameObject ItemInfoObject = null,
        GameObject parentObject = null
    )
    {
        var obj = Object.Instantiate(prefab, pos, Quaternion.identity);
        obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid);
        obj.TryGetComponent<DropItem>(out DropItem item);
        obj.TryGetComponent<MagnetItem>(out MagnetItem magnet);
        if (rigid == null || item == null)
            return;
        Vector3 force =
            type == DropType.Shop
                ? new Vector3(Random.Range(-Xforce, Xforce), Yforce, 0)
                : new Vector3(Xforce, Yforce, 0);
        item.item = data;
        rigid.AddForce(force);
        if (magnet != null)
            magnet.Amount = itemCount;
        if (ItemInfoObject != null)
        {
            obj.TryGetComponent<InteractionItem>(out InteractionItem interItem);
            interItem.InstItemInfo = ItemInfoObject;
            interItem.originObject = parentObject;
        }
    }
}

public struct ItemCreateWorker : IWorker
{
    public Item CreateItemWorker(List<Item> allItem, float common, float rare, float legend)
    {
        if (allItem == null || allItem.Count == 0)
            return null;

        ItemLevel targetRarity = DetermineRarity(common, rare, legend);

        int matchCount = 0;
        foreach (var item in allItem)
        {
            if (item.level == targetRarity)
                matchCount++;
        }

        if (matchCount == 0)
            return allItem[Random.Range(0, allItem.Count)];

        int randomIndex = Random.Range(0, matchCount);

        int currentIndex = 0;
        foreach (var item in allItem)
        {
            if (item.level == targetRarity)
            {
                if (currentIndex == randomIndex)
                    return item;
                currentIndex++;
            }
        }

        return null;
    }

    private ItemLevel DetermineRarity(float c, float r, float l)
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue < c)
            return ItemLevel.Common;
        if (randomValue < c + r)
            return ItemLevel.Rare;
        return ItemLevel.Legend;
    }
}

public struct ProbabilityWorker : IWorker
{
    public (float c, float r, float l) GetLevelBasedProb(
        int level,
        int maxLevel,
        float[] maxProb,
        float[] minProb
    )
    {
        float t = Mathf.Clamp01((float)(level - 1) / (maxLevel - 1));

        float common = Mathf.Lerp(minProb[0], maxProb[0], t);
        float rare = Mathf.Lerp(minProb[1], maxProb[1], t);

        float legend = 100 - (common + rare);
        return (common, rare, legend);
    }
}
