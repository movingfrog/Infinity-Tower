using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithSystem : MonoBehaviour
{
    public static BlackSmithSystem Instance;

    [Header("UI 속성")]
    public Slot[] AnvilInvenSlots;
    public InvenItem[] allItem = new InvenItem[13];
    public Image[] upgradeInfo;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI[] gettingGoods;
    public TextMeshProUGUI[] usingGoods;

    private const int InvenStart = 0;
    private const int WeaponStart = 9;
    private const int AccessoryStart = 11;
    private const int AnvilSlotStart = 13;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        getItem();
        refreshAllSlot();
    }

    private void OnDisable()
    {
        giveItem();
    }

    private void getItem()
    {
        for (int i = 0; i < AnvilSlotStart; i++)
        {
            allItem[i] = InventoryManager.Instance.allItem[i];
        }
    }

    private void giveItem()
    {
        if (allItem[AnvilSlotStart].item != null)
        {
            for (int i = 0; i < AnvilSlotStart; i++)
            {
                if (allItem[i].item == null)
                {
                    swapItem(i, AnvilSlotStart);
                }
            }
            if (allItem[AnvilSlotStart].item != null)
            {
                Debug.LogError(
                    "아직 구현 안됨 강화 탭에 넣은 상태로 끄면 떨어트리는 로직 구현 필요"
                );
            }
        }
        for (int i = 0; i < AnvilSlotStart; i++)
        {
            InventoryManager.Instance.allItem[i] = allItem[i];
        }

        InventoryManager.Instance.equipAccessories();
    }

    private void refreshAllSlot()
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

    bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        SlotType targetType = AnvilInvenSlots[targetIndex].type;

        if (targetType == SlotType.Inventory)
            return true;

        return targetType == draggingItem.item.slotType;
    }

    public void swapItem(int startIndex, int targetIndex)
    {
        if (!canPlace(targetIndex, allItem[startIndex]))
            return;

        InvenItem temp = allItem[startIndex];
        allItem[startIndex] = allItem[targetIndex];
        allItem[targetIndex] = temp;

        refreshAllSlot();
    }

    private void AnvilUIRefresh(uint gold, uint upgradeStone, int useGold, int useStone, Item item)
    {
        gettingGoods[0].text = gold.ToString("0");
        gettingGoods[1].text = upgradeStone.ToString("0");
        usingGoods[0].text = useGold.ToString("0");
        usingGoods[1].text = useStone.ToString("0");

        if (item != null)
        {
            upgradeInfo[0].sprite = item.spriteImage;
            upgradeInfo[1].sprite = item.Equips.nextItem.spriteImage;
            upgradeText.text = item.Equips.anvilInfoLine;
        }
        else
        {
            upgradeInfo[0].sprite = null;
            upgradeInfo[1].sprite = null;
            upgradeText.text = "";
        }
    }

    public void upgradeEquipment()
    {
        if (allItem[AnvilSlotStart].item != null) { }
    }
}
