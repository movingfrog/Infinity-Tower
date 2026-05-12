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
        int itemCount = 0
    )
    {
        var obj = Object.Instantiate(prefab, pos, Quaternion.identity);
        obj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid);
        obj.TryGetComponent<DropItem>(out DropItem item);
        obj.TryGetComponent<MagnetItem>(out MagnetItem magnet);
        if (rigid == item)
            return;
        Vector3 force =
            type == DropType.Shop
                ? new Vector3(Random.Range(-Xforce, Xforce), Yforce, 0)
                : new Vector3(Xforce, Yforce, 0);
        item.item = data;
        rigid.AddForce(force);
        if (magnet != null)
            magnet.Amount = itemCount;
    }
}
