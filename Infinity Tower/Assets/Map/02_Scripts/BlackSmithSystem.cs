using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithSystem : InvenParent
{
    Animator UpgradeAnimation;

    [Header("인벤 속성")]
    public Slot[] AnvilInvenSlots;
    public InvenItem[] allItem = new InvenItem[14];

    [Header("UI 속성")]
    public GameObject Panel;
    public Image[] upgradeInfo;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI[] gettingGoods;
    public TextMeshProUGUI[] usingGoods;

    GameObject DroppedItem;

    private const int InvenStart = 0;
    private const int WeaponStart = 9;
    private const int AccessoryStart = 11;
    private const int AnvilSlotStart = 13;

    private void Awake()
    {
        UpgradeAnimation = GetComponent<Animator>();
        for (int i = 0; i < AnvilInvenSlots.Length; i++)
            AnvilInvenSlots[i].invenManager = this;
    }

    private void OnEnable()
    {
        Panel.SetActive(false);
        getItem();
        RefreshAllSlot();
    }

    private void OnDisable()
    {
        giveItem();
    }

    private void Start() => DroppedItem = GameManager.Instance.ItemPrefab;

    private void getItem()
    {
        for (int i = 0; i < AnvilSlotStart; i++)
        {
            allItem[i] = InventoryManager.Instance.allItem[i];
        }
    }

    private void giveItem()
    {
        RemoveInven();
        for (int i = 0; i < AnvilSlotStart; i++)
        {
            InventoryManager.Instance.allItem[i] = allItem[i];
        }

        InventoryManager.Instance.RefreshAllSlot();
        InventoryManager.Instance.equipAccessories();
    }

    public override void RefreshAllSlot()
    {
        for (int i = 0; i < AnvilInvenSlots.Length; i++)
        {
            AnvilInvenSlots[i].refrashUI(allItem[i]);
        }
        if (allItem[AnvilSlotStart].item != null)
            AnvilUIRefresh(0, 0, 0, 0, allItem[AnvilSlotStart].item);
        else
            AnvilUIRefresh(0, 0, 0, 0, null);
    }

    public override bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        SlotType targetType = AnvilInvenSlots[targetIndex].type;

        if (targetType == SlotType.Inventory)
            return true;

        return targetType == draggingItem.item.slotType
            || (
                draggingItem.item.level != ItemLevel.Legend
                && draggingItem.item.slotType != SlotType.Inventory
                && targetType == SlotType.Anvil
            );
    }

    public override void swapItem(int startIndex, int targetIndex)
    {
        if (allItem[startIndex].item == null || !canPlace(targetIndex, allItem[startIndex]))
            return;

        (allItem[startIndex], allItem[targetIndex]) = (allItem[targetIndex], allItem[startIndex]);

        RefreshAllSlot();
    }

    public override RectTransform CanvasTransform() => GetComponent<RectTransform>();

    public override void DroppingItem() { }

    private void RemoveInven()
    {
        if (allItem[AnvilSlotStart].item != null)
        {
            for (int i = 0; i < AnvilSlotStart; i++)
            {
                if (allItem[i].item == null)
                {
                    swapItem(AnvilSlotStart, i);
                }
            }
            if (allItem[AnvilSlotStart].item != null)
            {
                WorkerHub<ItemDropWorker>.Instance.DropItemWork(
                    DroppedItem,
                    allItem[AnvilSlotStart].item,
                    transform.position,
                    DropType.Inventory,
                    -1
                );
            }
        }
    }

    private void AnvilUIRefresh(uint gold, uint upgradeStone, int useGold, int useStone, Item item)
    {
        gettingGoods[0].text = gold.ToString("0");
        gettingGoods[1].text = upgradeStone.ToString("0");
        usingGoods[0].text = useGold.ToString("0");
        usingGoods[1].text = useStone.ToString("0");

        if (item != null)
        {
            for (int i = 0; i < upgradeInfo.Length; i++)
                upgradeInfo[i].color = Color.white;
            upgradeInfo[0].sprite = item.spriteImage;
            upgradeInfo[1].sprite = item.Equips.nextItem.spriteImage;
            upgradeText.text = item.Equips.anvilInfoLine;
        }
        else
        {
            for (int i = 0; i < upgradeInfo.Length; i++)
                upgradeInfo[i].color = new Color(0, 0, 0, 0);
            upgradeInfo[0].sprite = null;
            upgradeInfo[1].sprite = null;
            upgradeText.text = "";
        }
    }

    public void upgradeEquipment()
    {
        if (allItem[AnvilSlotStart].item != null)
        {
            Dictionary<GoodsType, uint> goodsPrice = PriceTable.UpgradePrice(
                allItem[AnvilSlotStart].item.level
            );

            if (!InventoryManager.Instance.UseGoods(GoodsType.Gold, goodsPrice[GoodsType.Gold]))
                return;
            if (!InventoryManager.Instance.UseGoods(GoodsType.Stone, goodsPrice[GoodsType.Stone]))
                return;

            Panel.SetActive(true);
            UpgradeAnimation.SetTrigger("isAnvil");
        }
    }

    public void Upgrade()
    {
        //아이템의 추가 효과 적용 처리 필요
        //ex) 공격 유도, 범위 증가, 근접 공격 범위 내 투사체 삭제
        Panel.SetActive(false);
        allItem[AnvilSlotStart].item = allItem[AnvilSlotStart].item.Equips.nextItem;
        if (allItem[AnvilSlotStart].item.level == ItemLevel.Legend)
            RemoveInven();
        RefreshAllSlot();
    }

    public void BackToGame()
    {
        PlayerStatManager.instance.resetState();
        gameObject.SetActive(false);
    }
}
