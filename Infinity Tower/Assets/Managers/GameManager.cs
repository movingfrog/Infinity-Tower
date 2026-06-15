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
    public CinemachineConfiner2D confiner;

    [Header("ItemDropWorker용 변수"), Tooltip("떨어진 아이템 Prefab을 할당해주세요")]
    public GameObject ItemPrefab;

    [Tooltip("떨어진 전리품 Prefab을 할당 해주세요")]
    public GameObject LootPrefab;

    [Header("ItemCreateWorker용 변수")]
    public float maxCommonProb;
    public float minCommonProb;
    public float maxRareProb;
    public float minRareProb;

    public float[] maxProb() => new float[2] { maxCommonProb, maxRareProb };

    public float[] minProb() => new float[2] { minCommonProb, minRareProb };

    [Tooltip("모든 장비 SO를 할당해주세요")]
    public List<Item> allEquip;

    [Header("효과음 및 배경음악")]
    public AudioSource Source;
    public AudioClip[] SFX;
    public AudioClip[] BGM;
}
