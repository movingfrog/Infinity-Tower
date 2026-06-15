using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField]
    private Transform RewardPos;

    [SerializeField]
    private parentEnemy[] AllEnemy;

    private bool isDestroyed = false;

    private void LateUpdate()
    {
        if (isDestroyed)
            return;

        foreach (var e in AllEnemy)
        {
            if (e != null)
                return;
        }

        isDestroyed = true;
        SpawnReward();
        Destroy(gameObject);
    }

    private void SpawnReward()
    {
        if (GameManager.Instance == null || GameManager.Instance.AllRewardChest == null)
            return;

        GameObject randObject = WorkerHub<GetRandomReward>.Instance.RandRewardWorker(
            GameManager.Instance.AllRewardChest
        );

        if (randObject != null && RewardPos != null)
        {
            Instantiate(randObject, RewardPos.position, Quaternion.identity, null);
        }
    }
}
