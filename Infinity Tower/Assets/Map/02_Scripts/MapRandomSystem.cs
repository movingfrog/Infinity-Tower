using System.Collections.Generic;
using UnityEngine;

public class MapRandomSystem : MonoBehaviour
{
    [SerializeField]
    private int MAX_MAPCOUNT;

    [SerializeField]
    private int minNonEventMapCount;

    [SerializeField]
    private int maxNonEventMapCount;

    [Space(10f), SerializeField]
    private List<GameObject> AllShopMap;

    [SerializeField]
    private List<GameObject> AllStageMap;

    [SerializeField]
    private List<GameObject> AllEventMap;

    [Space(10f), SerializeField]
    private Transform ShopPos;

    [SerializeField]
    private Transform[] MapPos;

    private void Start()
    {
        SpawnShop();
        SpawnMap();
    }

    private void SpawnMap()
    {
        uint stageCount = (uint)Random.Range(minNonEventMapCount, maxNonEventMapCount + 1);
        uint eventCount = (uint)(MAX_MAPCOUNT - stageCount);
        List<GameObject> allStage = WorkerHub<GetRandomMap>.Instance.RandMapWorker(
            AllStageMap,
            stageCount
        );
        List<GameObject> allEvent = WorkerHub<GetRandomMap>.Instance.RandMapWorker(
            AllEventMap,
            eventCount
        );

        allStage.AddRange(allEvent);
        ShuffleList(allStage);

        foreach (var Map in MapPos)
        {
            GameObject SpawnMap = Instantiate(allStage[0], Map);
            allStage.RemoveAt(0);
            SpawnMap.transform.localPosition = Vector3.zero;
        }
    }

    private void SpawnShop()
    {
        GameObject shop = Instantiate(
            WorkerHub<GetRandomMap>.Instance.RandMapWorker(AllShopMap, 1)[0],
            ShopPos
        );
        shop.transform.localPosition = Vector3.zero;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
