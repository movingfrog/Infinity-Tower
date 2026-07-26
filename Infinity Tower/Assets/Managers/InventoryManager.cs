using System;
using System.Diagnostics;
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
    GameObject DroppedItem;
    GameObject DroppedLoot;

    public static InventoryManager Instance { get; private set; }

    [Header("인벤 열고 닫는 기능")]
    public GameObject Inven;

    [Header("재화")]
    public SO_Goods[] Goods;
    public TextMeshProUGUI[] GoodsText;

    [Header("인벤 주요 기능")]
    public Slot[] allSlot;
    public InvenItem[] allItem = new InvenItem[17];

    public event Action<Item> EquipEvent;
    public event Func<Item, Item, bool> UnEquipEvent;
    public event Func<Item, bool> ChangeEvent;

    private int currentWeaponCount;

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
        DroppedItem = GameManager.Instance.ItemPrefab;
        DroppedLoot = GameManager.Instance.LootPrefab;
        UseInEditor();
    }

    [Conditional("UNITY_EDITOR")]
    private void UseInEditor()
    {
        for (int i = 0; i < Goods.Length; i++)
        {
            Goods[i].Decrease(Goods[i].Get);
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Inven.started += OnInven;
        InputManager.Instance.inputActions.Player.WeaponChange.started += ChangeWeapon;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Inven.started -= OnInven;
        InputManager.Instance.inputActions.Player.WeaponChange.started -= ChangeWeapon;
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
            int limit = Mathf.Min(Goods.Length, GoodsText.Length);
            for (int i = 0; i < limit; i++)
            {
                if (Goods[i] != null && GoodsText[i] != null)
                {
                    GoodsText[i].text = Goods[i].Get.ToString("0");
                    if (Goods[i].Type == GoodsType.Gold)
                        GoodsText[i].text += "G";
                }
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
        if (
            allSlot[startIndex].type == SlotType.Weapon
            && allSlot[targetIndex].type != SlotType.Weapon
            && allItem[WEAPON_START == startIndex ? WEAPON_START + 1 : WEAPON_START].item == null
        )
            return;
        (allItem[startIndex], allItem[targetIndex]) = (allItem[targetIndex], allItem[startIndex]);

        if (allSlot[targetIndex].type == SlotType.Accessories) // 옮기는 아이템을 액세서리 슬롯에 장착할 경우 실행
            EquipAccessories();
        if (allSlot[targetIndex].type == SlotType.Weapon) // 옮기는 아이템을 무기 슬롯에 장착할 경우 실행
        {
            if (
                allItem[WEAPON_START + currentWeaponCount].item != null
                && allItem[WEAPON_START + (1 - currentWeaponCount)].item == null
            ) // 현재 가리키는 무기가 장착되어있다면 실행
                currentWeaponCount = currentWeaponCount > 0 ? 0 : 1;
            EquipWeapon(allItem[targetIndex]?.item);
        }
        if (
            allSlot[startIndex].type == SlotType.Weapon
            && allSlot[targetIndex].type != SlotType.Weapon
        ) // 무기 슬롯에서 다른 슬롯으로 옮길 경우 실행
            UnEquipWeapon(allItem[targetIndex]?.item, allItem[WEAPON_START + currentWeaponCount].item);
        RefreshAllSlot();
    }

    public override RectTransform CanvasTransform() => GetComponentInChildren<RectTransform>();

    public override void DroppingItem() { }

    public void EquipAccessories()
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

    public void EquipWeapon(Item weaponItem)
    {
        UnityEngine.Debug.Log("무기 장착");

        EquipEvent?.Invoke(weaponItem);
    }

    public void UnEquipWeapon(Item targetItem, Item OtherEquipItem)
    {
        UnityEngine.Debug.Log("무기 해제");

        if (OtherEquipItem != null && UnEquipEvent?.Invoke(targetItem, OtherEquipItem) == true)
        {
            currentWeaponCount = currentWeaponCount > 0 ? 0 : 1;
        }
    }

    public void ChangeWeapon(InputAction.CallbackContext callback)
    {
        var weaponItem = allItem[WEAPON_START + currentWeaponCount];
        if (
            weaponItem.item != null
            && ChangeEvent?.Invoke(allItem[WEAPON_START + currentWeaponCount].item) == true
        )
        {
            currentWeaponCount = currentWeaponCount > 0 ? 0 : 1;
        }
    }

    public void GetGoods(GoodsType type, uint amount) =>
        Goods[(int)type].Increase((uint)(amount * PlayerStatManager.instance.GoldBoost));

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
