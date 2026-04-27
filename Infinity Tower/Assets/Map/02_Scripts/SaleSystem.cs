using UnityEngine;

public class SaleSystem : InvenParent
{
    Animator SaleAnimation;

    [Header("인벤 속성")]
    public Slot[] AllSlots;
    public InvenItem[] AllItem = new InvenItem[22];

    private const int InventorySlotEnd = 9;
    private const int SaleSlotStart = 13;

    private void Awake()
    {
        SaleAnimation = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        getItem();
        RefreshAllSlot();
    }

    private void getItem()
    {
        for (int i = 0; i < SaleSlotStart; i++)
        {
            AllItem[i] = InventoryManager.Instance.allItem[i];
        }
    }

    public void Sale()
    {
        SaleAnimation.SetTrigger("Sale");
        for (int i = 0; i < InventorySlotEnd; i++)
        {
            if (AllItem[i].item == null)
                continue;
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
}
