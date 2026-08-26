using UnityEngine;

[CreateAssetMenu(
    fileName = "RewardBossItem",
    menuName = "Scriptable Objects/Reward/RewardBossItem"
)]
public class RewardBossItem : RewardTable
{
    public GameObject RewardGoodsObject;

    public override void ExcuteSpawn(GameObject prefabObject, Transform parent)
    {
        int randomAmount = Random.Range(minRewardAmount, maxRewardAmount);
        for (int i = 0; i < randomAmount; i++)
        {
            Instantiate(RewardGoodsObject, parent.position, Quaternion.identity);
            Instantiate(prefabObject, parent.position, Quaternion.identity);
        }
    }
}
