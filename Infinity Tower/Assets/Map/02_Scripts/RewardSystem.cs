using UnityEngine;

public class RewardSystem : MonoBehaviour
{
    [SerializeField]
    private Transform RewardPos;

    [SerializeField]
    private parentEnemy[] AllEnemy;

    private void LateUpdate()
    {
        foreach (var e in AllEnemy)
        {
            if (e != null)
                return;
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject randObject = WorkerHub<GetRandomReward>.Instance.RandRewardWorker(
            GameManager.Instance.AllRewardChest
        );
        Instantiate(randObject, RewardPos.position, Quaternion.identity, null);
    }
}
