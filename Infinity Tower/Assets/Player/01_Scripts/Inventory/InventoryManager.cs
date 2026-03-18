using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

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

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("인벤 열고 닫는 기능")]
    public GameObject Inven;
    [Header("인벤 주요 기능")]
    public Slot[] allSlot;
    public InvenItem[] allItem = new InvenItem[17];
    private const int INVEN_START = 0;
    private const int WEAPON_START = 9;
    private const int ACCESSORY_START = 11;
    private const int IMPORT_START = 13;


    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void OnInven()
    {
        Inven.SetActive(!Inven.activeSelf);
    }

    bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        SlotType targetType = allSlot[targetIndex].type;

        if (targetType == SlotType.Inventory) return true;

        return targetType == draggingItem.item.slotType;
    }

    public void swapItem(int startIndex, int targetIndex)
    {
        if (!canPlace(targetIndex, allItem[startIndex])) return;

        InvenItem temp = allItem[startIndex];
        allItem[startIndex] = allItem[targetIndex];
        allItem[targetIndex] = temp;

        if (allSlot[targetIndex].type == SlotType.Accessories) equipAccessories();
        refreshAllSlot();
    }
    void equipAccessories()
    {
        PlayerStatManager.instance.resetStat();
        if (allItem[ACCESSORY_START].item != null) for (int i = 0; i < allItem[ACCESSORY_START].item.Equips.statModifiers.Count; i++) 
                PlayerStatManager.instance.statUp(allItem[ACCESSORY_START].item.Equips.statModifiers[i].Type, allItem[ACCESSORY_START].item.Equips.statModifiers[i].Value);
        if (allItem[ACCESSORY_START + 1].item != null) for (int i = 0; i < allItem[ACCESSORY_START + 1].item.Equips.statModifiers.Count; i++)
                PlayerStatManager.instance.statUp(allItem[ACCESSORY_START + 1].item.Equips.statModifiers[i].Type, allItem[ACCESSORY_START + 1].item.Equips.statModifiers[i].Value);
    }
    void refreshAllSlot()
    {
        for(int i = 0; i < allSlot.Length; i++)
        {
            allSlot[i].refrashUI(allItem[i]);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
