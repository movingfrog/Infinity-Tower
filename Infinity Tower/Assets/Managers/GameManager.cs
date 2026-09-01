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

    [Header("CamMoveWorker용 변수")]
    private CinemachineConfiner2D _confiner;
    public CinemachineConfiner2D confiner
    {
        get
        {
            if (_confiner == null)
                _confiner = FindAnyObjectByType<CinemachineConfiner2D>();
            return _confiner;
        }
        set { _confiner = value; }
    }

    [Header("ItemDropWorker용 변수")]
    [field: SerializeField, Tooltip("떨어진 아이템 Prefab을 할당해주세요")]
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

    [Header("모든 무기 GUID")]
    public List<System.Guid> allWeaponGuid { get; private set; } = new List<System.Guid>();
    public Dictionary<System.Guid, WeaponObjectData> allWeaponData { get; private set; } =
        new Dictionary<System.Guid, WeaponObjectData>();

    [Header("DropEnchant용 변수"), Tooltip("상자에서 떨어진 각인 Prefab을 할당해주세요")]
    [field: SerializeField]
    public GameObject ChestDropEnchantObject { get; private set; }

    [Tooltip("인벤토리에서 떨어진 각인 Prefab을 할당해주세요")]
    [field: SerializeField]
    public GameObject InvenDropEnchantObject { get; private set; }

    [Header("모든 각인 SO")]
    [field: SerializeField]
    public List<WeaponEnchant> allEnchant { get; private set; }
}
