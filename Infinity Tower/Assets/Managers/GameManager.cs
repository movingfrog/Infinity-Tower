using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    [field: SerializeField]
    public CinemachineConfiner2D confiner { get; private set; }

    [Header("ItemDropWorker용 변수"), Tooltip("떨어진 아이템 Prefab을 할당해주세요")]
    [field: SerializeField]
    public GameObject ItemPrefab { get; private set; }

    [Tooltip("떨어진 전리품 Prefab을 할당 해주세요")]
    [field: SerializeField]
    public GameObject LootPrefab { get; private set; }

    [Header("ItemCreateWorker용 변수")]
    [field: SerializeField]
    public float maxCommonProb { get; private set; }

    [field: SerializeField]
    public float minCommonProb { get; private set; }

    [field: SerializeField]
    public float maxRareProb { get; private set; }

    [field: SerializeField]
    public float minRareProb { get; private set; }

    public float[] maxProb() => new float[2] { maxCommonProb, maxRareProb };

    public float[] minProb() => new float[2] { minCommonProb, minRareProb };

    [Tooltip("모든 장비 SO를 할당해주세요")]
    [field: SerializeField]
    public List<Item> allEquip { get; private set; }

    [Header("효과음 및 배경음악")]
    public AudioSource Source;
    public AudioClip[] SFX;
    public AudioClip[] BGM;

    [Header("모든 보상 상자")]
    [field: SerializeField]
    public GameObject[] AllRewardChest { get; private set; }
}
