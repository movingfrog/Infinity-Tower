using UnityEngine;

[CreateAssetMenu(fileName = "RewardGoods", menuName = "Scriptable Objects/Reward/RewardGoods")]
public class RewardGoods : RewardTable
{
    [Header("물리 작용")]
    [SerializeField]
    private float minXForce;

    [SerializeField]
    private float maxXForce;

    [SerializeField]
    private float yForce;

    public override void ExcuteSpawn(GameObject prefabObject, Transform parent)
    {
        if (prefabObject == null || parent == null)
            return;

        Instantiate(prefabObject, parent.position, Quaternion.identity);
    }
}
