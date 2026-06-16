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
        int stageCount = Random.Range(minNonEventMapCount, maxNonEventMapCount + 1);
        int eventCount = Mathf.Max(0, MAX_MAPCOUNT - stageCount);
        List<GameObject> allStage = WorkerHub<GetRandomMap>.Instance.RandMapWorker(
            AllStageMap,
            (uint)stageCount
        );
        List<GameObject> allEvent = WorkerHub<GetRandomMap>.Instance.RandMapWorker(
            AllEventMap,
            (uint)eventCount
        );

        allStage.AddRange(allEvent);
        ShuffleList(allStage);

        for (int i = 0; i < MapPos.Length; i++)
        {
            if (i >= allStage.Count)
            {
                Debug.LogError("생성된 맵이 개수가 너무 많습니다");
                break;
            }
            GameObject spawnedMap = Instantiate(allStage[i], MapPos[i]);
            spawnedMap.transform.localPosition = Vector3.zero;
        }
    }

    private void SpawnShop()
    {
        List<GameObject> shopMaps = WorkerHub<GetRandomMap>.Instance.RandMapWorker(AllShopMap, 1);
        if (shopMaps == null || shopMaps.Count == 0)
        {
            Debug.Log("상점이 생성 되지 않았습니다");
            return;
        }

        GameObject shop = Instantiate(shopMaps[0], ShopPos);
        shop.transform.localPosition = Vector3.zero;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
