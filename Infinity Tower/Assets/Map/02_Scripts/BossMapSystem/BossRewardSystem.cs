using UnityEngine;

public class BossRewardSystem : RewardSystem
{
    protected override void SpawnReward()
    {
        if (RewardPos != null)
        {
            RewardPos.gameObject.SetActive(true);
        }
    }
}
