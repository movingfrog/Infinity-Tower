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
    public Slot[] invenSlot;
    public Slot[] equipSlot;
    public Slot[] importSlot;
    public InvenItem[] invenItem = new InvenItem[9];
    public Item[] equipItem = new Item[4];
    public Item[] importItem = new Item[4];


    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public void OnInven()
    {
        Inven.SetActive(!Inven.activeSelf);
    }

    public void swapItem(int startIndex, int targetIndex, SlotType targetType, SlotType type)
    {
        switch (targetType)
        {
            case SlotType.Inventory:
                if(type == SlotType.Inventory)
                {
                    InvenItem temp = invenItem[startIndex];
                    invenItem[startIndex] = invenItem[targetIndex];
                    invenItem[targetIndex] = temp;
                }
                else
                {
                    if ( invenItem[targetIndex].item == null || invenItem[targetIndex].item.isWearable)
                    {
                        InvenItem temp = new InvenItem(equipItem[startIndex], 1);
                        equipItem[startIndex] = invenItem[targetIndex].item;
                        invenItem[targetIndex] = temp;
                    }
                }
                break;
            case SlotType.Weapon:
                if (invenItem[startIndex].item.isWearable)
                {
                    InvenItem weaponTemp = new InvenItem(equipItem[targetIndex], 1);
                    equipItem[targetIndex] = invenItem[startIndex].item;
                    invenItem[startIndex] = weaponTemp;
                }
                break;
            case SlotType.Accessory:
                if (invenItem[startIndex].item.isWearable)    
                {
                    InvenItem accessoryTemp = new InvenItem(equipItem[targetIndex], 1);
                    equipItem[targetIndex] = invenItem[startIndex].item;
                    invenItem[startIndex] = accessoryTemp;
                    equipAccessory();
                }
                break;
            case SlotType.Import:
                break;
        }

        refreshAllSlot();
    }
    void equipAccessory()
    {
        PlayerStatManager.instance.resetStat();
        if (equipItem[2] != null) for (int i = 0; i < equipItem[2].Equips.statModifiers.Count; i++) 
                PlayerStatManager.instance.statUp(equipItem[2].Equips.statModifiers[i].Type, equipItem[2].Equips.statModifiers[i].Value);
        if (equipItem[3] != null) for (int i = 0; i < equipItem[3].Equips.statModifiers.Count; i++)
                PlayerStatManager.instance.statUp(equipItem[3].Equips.statModifiers[i].Type, equipItem[3].Equips.statModifiers[i].Value);
    }
    void refreshAllSlot()
    {
        for(int i = 0; i < invenSlot.Length; i++)
        {
            invenSlot[i].refrashUI(invenItem[i].item, invenItem[i].currentItemCount);
        }
        for (int i = 0; i < equipSlot.Length; i++)
        {
            equipSlot[i].refrashUI(equipItem[i]);
        }
        for(int i = 0;i< importSlot.Length; i++)
        {
            importSlot[i].refrashUI(importItem[i]);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
