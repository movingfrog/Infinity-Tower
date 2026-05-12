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
    private int itemDropXForce;

    [SerializeField]
    private bool isHealth;

    private GameObject DroppedItem;
    private GameObject ItemInfoObject;
    private TextMeshProUGUI ItemInfoText;
    private Item sellItem;
    private bool hasItem;

    private void Start()
    {
        DroppedItem = GameManager.Instance.ItemPrefab;
        ItemInfoObject = SpaceUIManager.Instance.CreateItemUI(gameObject);
        ItemInfoText = ItemInfoObject.GetComponentInChildren<TextMeshProUGUI>();
        RefreshUI();
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

        ItemInfoObject.SetActive(outPlayer != null);
    }

    private void RefreshUI()
    {
        string infoLine = sellItem.itemName + $"<color=>";
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
                    itemDropXForce
                );
                ItemImage.sprite = null;
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
