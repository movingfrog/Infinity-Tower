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
    public Slot[] weaponSlot;
    public Slot[] accessorySlot;
    public Slot[] importSlot;
    public InvenItem[] invenItem = new InvenItem[9];
    public Item[] weaponItem = new Item[2];
    public Item[] accessoryItem = new Item[2];
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
                    //하다가 만 곳
                    //만들어야 할 것
                    //1. weapon과 accessory배열 합치기
                    //2. 시작이 inventory가 아닐 때 새 invenitem을 생성해서 끼워주기 만들기
                    //InvenItem temp = new InvenItem()
                }
                break;
            case SlotType.Weapon:
                InvenItem weaponTemp = new InvenItem(weaponItem[targetIndex], 0);
                weaponItem[targetIndex] = invenItem[startIndex].item;
                invenItem[startIndex] = weaponTemp;
                break;
            case SlotType.Accessory:


                equipAccessory();
                break;
            case SlotType.Import:
                break;
        }

        refreshAllSlot();
    }
    void equipAccessory()
    {
        PlayerStatManager.instance.resetStat();
        if (accessoryItem[0] != null) for (int i = 0; i < accessoryItem[0].Equips.statModifiers.Count; i++) 
                PlayerStatManager.instance.statUp(accessoryItem[0].Equips.statModifiers[i].Type, accessoryItem[0].Equips.statModifiers[i].Value);
        if (accessoryItem[1] != null) for (int i = 0; i < accessoryItem[1].Equips.statModifiers.Count; i++) 
                PlayerStatManager.instance.statUp(accessoryItem[1].Equips.statModifiers[i].Type, accessoryItem[1].Equips.statModifiers[i].Value);
    }
    void refreshAllSlot()
    {
        for(int i = 0; i < invenSlot.Length; i++)
        {
            invenSlot[i].refrashUI(invenItem[i].item, invenItem[i].currentItemCount);
        }
        for (int i = 0; i < weaponSlot.Length; i++)
        {
            weaponSlot[i].refrashUI(weaponItem[i]);
            accessorySlot[i].refrashUI(accessoryItem[i]);
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
