using UnityEngine;

[CreateAssetMenu(fileName = "RewardGoods", menuName = "Scriptable Objects/Reward/RewardGoods")]
public class RewardGoods : RewardTable
{
    public override void ExcuteSpawn(GameObject prefabObject, Transform parent)
    {
        if (prefabObject == null || parent == null)
            return;

        Instantiate(prefabObject, parent.position, Quaternion.identity);
    }
}
