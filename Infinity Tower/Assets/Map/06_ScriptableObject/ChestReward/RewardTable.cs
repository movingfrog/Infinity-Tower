using UnityEngine;

public enum RewardType
{
    Item,
    Goods,
}

public abstract class RewardTable : ScriptableObject
{
    [Header("보상 타입")]
    public RewardType Type;
    public GameObject RewardObject;

    [Header("보상 수량")]
    public int minRewardAmount = 1;
    public int maxRewardAmount = 1;

    public virtual void Reward(Transform parent)
    {
        int randomAmount = Random.Range(minRewardAmount, maxRewardAmount);

        for (int i = 0; i < randomAmount; i++)
        {
            ExcuteSpawn(RewardObject, parent);
        }
    }

    public abstract void ExcuteSpawn(GameObject prefabObject, Transform parent);
}
