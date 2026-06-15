using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public interface IWorker { }

/// <summary>
/// Worker구조체를 실행시키기 위한 제네릭 입니다. 필요한 구조체를 할당 후 사용해주시면 됩니다.
/// </summary>
/// <typeparam name="T">필요한 Worker구조체를 넣어 메서드를 실행시키면 됩니다</typeparam>
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

/// <summary>
/// 이 구조체는 사운드를 실행하기 위한 worker이다
/// </summary>
public struct SoundWorker : IWorker
{
    /// <summary>
    /// 이 메서드는 효과음을 실행하기 위한 메서드 입니다
    /// </summary>
    /// <param name="source">GameManager에 저장된 AudioSource를 할다하여 주세요</param>
    /// <param name="SFXClip">현재 필요한 효과음을 GamaManager에서 찾아서 할당아여 주세요</param>
    public void PlaySFX(AudioSource source, AudioClip SFXClip)
    {
        source.PlayOneShot(SFXClip);
    }

    /// <summary>
    /// 이 메서드는 배경음악을 재생하기 위한 메서드 입니다
    /// 주의: 한 번 실행한 후 다시 실행하면 이전에 BGM 사라짐
    /// </summary>
    /// <param name="source">GameManager에 저장된 AudioSource를 할다하여 주세요</param>
    /// <param name="BGMClip">현재 스테이지 혹은 보스의 배경음악을 GamaManager에서 찾아서 할당아여 주세요</param>
    public void PlayBGM(AudioSource source, AudioClip BGMClip)
    {
        source.clip = BGMClip;
        source.Play();
    }
}
