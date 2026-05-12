using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (Instance == null)
            {
                Debug.LogError(
                    "아직 할당 되지 않았습니다 Editor전 Initialization에서 호출을 없애주세요"
                );
                return null;
            }
            return Instance;
        }
        set { Instance = value; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("CamMoveWorker용 변수"), Tooltip("현재 씬의 Cinemachine을 할당해주세요")]
    public CinemachineConfiner2D confiner;

    [Header("ItemDropWorker용 변수"), Tooltip("떨어진 아이템 Prefab을 할당해주세요")]
    public GameObject ItemPrefab;

    [Tooltip("떨어진 전리품 Prefab을 할당 해주세요")]
    public GameObject LootPrefab;
}
