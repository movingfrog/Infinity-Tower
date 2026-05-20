using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopBox : MonoBehaviour
{
    [Header("플레이어 관련")]
    [SerializeField]
    private float getSize = 1;

    [SerializeField]
    private LayerMask Player;

    [Header("상점 관련")]
    [SerializeField]
    private SpriteRenderer ItemImage;

    [SerializeField]
    private float itemDropXForce;

    [SerializeField]
    private bool isHealth;

    [Header("오브젝트 속성")]
    private TextMeshProUGUI ItemInfoText;
    private Item sellItem;
    private bool hasItem;

    [Header("GameManager할당 받을 값")]
    private List<Item> allEquipItem;
    private float[] maxProb = new float[2];
    private float[] minProb = new float[2];
    private int MaxLevel;
    private GameObject DroppedItem;
    private GameObject ItemInfoObject;

    private void Start()
    {
        DroppedItem = GameManager.Instance.ItemPrefab;
        allEquipItem = GameManager.Instance.allEquip;
        maxProb = GameManager.Instance.maxProb();
        minProb = GameManager.Instance.minProb();
        MaxLevel = PlayerStatManager.instance.maxLevel;
        ItemInfoObject = SpaceUIManager.Instance.CreateItemUI(gameObject);
        ItemInfoText = ItemInfoObject.GetComponentInChildren<TextMeshProUGUI>();
        GetNewItem();
        ItemInfoObject.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += Buy;
        if (ItemInfoObject != null)
            RefreshUI();
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= Buy;
    }

    private void FixedUpdate()
    {
        Collider2D outPlayer = Physics2D.OverlapCircle(transform.position, getSize, Player);
        bool isPlayer =
            outPlayer != null
            && (outPlayer.transform.position - transform.position).magnitude <= getSize;

        if (ItemInfoObject != null)
            ItemInfoObject.SetActive(isPlayer);
    }

    public void GetNewItem()
    {
        (float c, float r, float l) = WorkerHub<ProbabilityWorker>.Instance.GetLevelBasedProb(
            PlayerStatManager.instance.Level,
            MaxLevel,
            maxProb,
            minProb
        );
        sellItem = WorkerHub<ItemCreateWorker>.Instance.CreateItemWorker(allEquipItem, c, r, l);
        hasItem = true;
        if (ItemInfoObject == null)
        {
            ItemInfoObject = SpaceUIManager.Instance.CreateItemUI(gameObject);
            ItemInfoText = ItemInfoObject.GetComponentInChildren<TextMeshProUGUI>();
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        string infoLine =
            sellItem.itemName
            + (!isHealth ? " <color=yellow>G" : " <color=red>♥")
            + PriceTable.GetPrice(sellItem.level, isHealth);
        ItemInfoText.color = SpaceUIManager.Instance.rarityColor[(int)sellItem.level];
        ItemInfoText.text = infoLine;
        ItemImage.sprite = sellItem.spriteImage;
    }

    private void Buy(InputAction.CallbackContext context)
    {
        if (hasItem && ItemInfoObject.activeSelf)
        {
            uint price = PriceTable.GetPrice(sellItem.level, isHealth);
            bool isPaymentSuccess = false;
            if (!isHealth)
                isPaymentSuccess = InventoryManager.Instance.UseGoods(GoodsType.Gold, price);
            else
                isPaymentSuccess = PlayerStatManager.instance.DecreassHealth(price);

            if (isPaymentSuccess)
            {
                WorkerHub<ItemDropWorker>.Instance.DropItemWork(
                    DroppedItem,
                    sellItem,
                    transform.position,
                    DropType.Shop,
                    itemDropXForce,
                    .25f,
                    1,
                    ItemInfoObject,
                    gameObject
                );
                ItemImage.sprite = null;
                ItemInfoObject = null;
                sellItem = null;
                hasItem = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, getSize);
    }
}
