using UnityEngine;

public class SaleSystem : InvenParent
{
    GameObject DroppedItem;
    GameObject DroppedLoot;
    Animator SaleAnimation;

    [Header("인벤 속성")]
    public Slot[] AllSlots;
    public InvenItem[] AllItem = new InvenItem[22];

    private const int InventorySlotEnd = 9;
    private const int SaleSlotStart = 13;
    private const int SaleSlotEnd = 22;

    private void Awake()
    {
        SaleAnimation = GetComponent<Animator>();
    }

    private void Start()
    {
        DroppedItem = GameManager.Instance.ItemPrefab;
        DroppedLoot = GameManager.Instance.LootPrefab;
    }

    private void OnEnable()
    {
        getItem();
        RefreshAllSlot();
    }

    private void OnDisable()
    {
        GiveItem();
    }

    private void getItem()
    {
        for (int i = 0; i < SaleSlotStart; i++)
        {
            AllItem[i] = InventoryManager.Instance.allItem[i];
        }
    }

    private void GiveItem()
    {
        RemoveInven();
        for (int i = 0; i < SaleSlotStart; i++)
        {
            InventoryManager.Instance.allItem[i] = AllItem[i];
        }

        InventoryManager.Instance.RefreshAllSlot();
        InventoryManager.Instance.equipAccessories();
    }

    private void RemoveInven()
    {
        if (CheckSaleSlot)
        {
            for (int i = 0; i < SaleSlotStart; i++)
            {
                if (AllItem[i].item == null)
                {
                    swapItem(SaleSlotStart, i);
                }
            }
            for (int i = SaleSlotStart; i < AllSlots.Length; i++)
            {
                if (AllItem[i].item != null)
                {
                    WorkerHub<ItemDropWorker>.Instance.DropItemWork(
                        AllItem[i].item.isEquippable ? DroppedItem : DroppedLoot,
                        AllItem[i].item,
                        transform.position,
                        DropType.Inventory,
                        1,
                        .25f,
                        AllItem[i].item.isEquippable ? 0 : AllItem[i].currentItemCount
                    );
                }
            }
        }
    }

    bool CheckSaleSlot
    {
        get
        {
            for (int i = SaleSlotStart; i < AllSlots.Length; i++)
            {
                if (AllItem[i].item != null)
                    return true;
            }
            return false;
        }
    }

    public void Sale()
    {
        SaleAnimation.SetTrigger("Sale");
    }

    public void SaleAnimationFuc()
    {
        for (int i = SaleSlotStart; i < SaleSlotEnd; i++)
        {
            if (AllItem[i].item == null)
                continue;
            Debug.Log(
                (uint)PriceTable.GetSellPrice(AllItem[i].item.level, AllItem[i].item.isEquippable)
            );
            InventoryManager.Instance.GetGoods(
                GoodsType.Gold,
                (uint)PriceTable.GetSellPrice(AllItem[i].item.level, AllItem[i].item.isEquippable)
            );
            AllItem[i].item = null;
        }
        RefreshAllSlot();
    }

    public void BackToGame()
    {
        PlayerStatManager.instance.resetState();
        gameObject.SetActive(false);
    }

    public override RectTransform CanvasTransform() => GetComponent<RectTransform>();

    public override void RefreshAllSlot()
    {
        for (int i = 0; i < AllSlots.Length; i++)
        {
            AllSlots[i].refrashUI(AllItem[i]);
        }
    }

    public override bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        SlotType targetType = AllSlots[targetIndex].type;

        if (targetType == SlotType.Inventory)
            return true;

        return targetType == draggingItem.item.slotType;
    }

    public override void swapItem(int startIndex, int targetIndex)
    {
        if (AllItem[startIndex].item == null || !canPlace(targetIndex, AllItem[startIndex]))
            return;

        (AllItem[startIndex], AllItem[targetIndex]) = (AllItem[targetIndex], AllItem[startIndex]);

        RefreshAllSlot();
    }

    public override void DroppingItem() { }
}
