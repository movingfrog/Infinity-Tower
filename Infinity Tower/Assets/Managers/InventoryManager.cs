using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class InvenItem
{
    public Item item;
    public int currentItemCount;

    public InvenItem(Item _item, int _itemCount)
    {
        item = _item;
        currentItemCount = _itemCount;
    }

    public void resetItem()
    {
        item = null;
        currentItemCount = 0;
    }
}

public class InventoryManager : InvenParent
{
    public static InventoryManager Instance;

    [Header("인벤 열고 닫는 기능")]
    public GameObject Inven;

    [Header("재화")]
    public SO_Goods[] Goods;
    public TextMeshProUGUI[] GoodsText;

    [Header("인벤 주요 기능")]
    public Slot[] allSlot;
    public InvenItem[] allItem = new InvenItem[17];
    private const int INVEN_START = 0;
    private const int WEAPON_START = 9;
    private const int ACCESSORY_START = 11;
    private const int IMPORT_START = 13;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
        for (int i = 0; i < allSlot.Length; i++)
            allSlot[i].invenManager = this;
    }

    private void Start()
    {
        RefreshAllSlot();
        Inven.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Inven.started += OnInven;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Inven.started -= OnInven;
    }

    public void OnInven(InputAction.CallbackContext callback)
    {
        if (
            !PlayerStatManager.instance.getState(PlayerState.Idle)
            && PlayerStatManager.instance.currentState != PlayerState.InvenOpen
        )
            return;
        if (PlayerStatManager.instance.getState(PlayerState.Idle))
        {
            for (int i = 0; i < Goods.Length; i++)
            {
                GoodsText[i].text = Goods[i].Get.ToString("0");
                if (Goods[i].Type == GoodsType.Gold)
                    GoodsText[i].text += "G";
            }
            Inven.SetActive(true);
            PlayerStatManager.instance.ChangeState(PlayerState.InvenOpen);
        }
        else
        {
            Inven.SetActive(false);
            PlayerStatManager.instance.resetState();
        }
    }

    public void GetItem(Item dropItem, int amount)
    {
        int i = INVEN_START;
        while (amount > 0 && i < WEAPON_START)
        {
            if (allItem[i].item == null)
            {
                int addCount = Mathf.Min(dropItem.MaxItemCount, amount);
                allItem[i] = new InvenItem(dropItem, addCount);
                amount -= addCount;
            }
            else if (allItem[i].item == dropItem && !dropItem.isEquippable)
            {
                int spaceLeft = dropItem.MaxItemCount - allItem[i].currentItemCount;
                if (spaceLeft > 0)
                {
                    int addCount = Mathf.Min(spaceLeft, amount);
                    allItem[i].currentItemCount += addCount;
                    amount -= addCount;
                }
            }

            i++;
        }

        RefreshAllSlot();
    }

    public override bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        SlotType targetType = allSlot[targetIndex].type;

        if (targetType == SlotType.Inventory)
            return true;

        return targetType == draggingItem.item.slotType;
    }

    public override void swapItem(int startIndex, int targetIndex)
    {
        if (allItem[startIndex].item == null || !canPlace(targetIndex, allItem[startIndex]))
            return;

        (allItem[startIndex], allItem[targetIndex]) = (allItem[targetIndex], allItem[startIndex]);

        if (allSlot[targetIndex].type == SlotType.Accessories)
            equipAccessories();
        RefreshAllSlot();
    }

    public override RectTransform CanvasTransform() => GetComponentInChildren<RectTransform>();

    public void equipAccessories()
    {
        PlayerStatManager.instance.resetStat();
        if (allItem[ACCESSORY_START].item != null)
            for (int i = 0; i < allItem[ACCESSORY_START].item.Equips.statModifiers.Count; i++)
                PlayerStatManager.instance.statUp(
                    allItem[ACCESSORY_START].item.Equips.statModifiers[i].Type,
                    allItem[ACCESSORY_START].item.Equips.statModifiers[i].Value
                );
        if (allItem[ACCESSORY_START + 1].item != null)
            for (int i = 0; i < allItem[ACCESSORY_START + 1].item.Equips.statModifiers.Count; i++)
                PlayerStatManager.instance.statUp(
                    allItem[ACCESSORY_START + 1].item.Equips.statModifiers[i].Type,
                    allItem[ACCESSORY_START + 1].item.Equips.statModifiers[i].Value
                );
    }

    public void GetGoods(GoodsType type, uint amount) => Goods[(int)type].Increase(amount);

    public bool UseGoods(GoodsType type, uint amount) => Goods[(int)type].Decrease(amount);

    public override void RefreshAllSlot()
    {
        for (int i = 0; i < allSlot.Length; i++)
        {
            allSlot[i].refrashUI(allItem[i]);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
