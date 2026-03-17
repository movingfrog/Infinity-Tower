using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InvenItem
{
    public Item item;
    public int currentItemCount;

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

    public void swapItem(int startIndex, int targetIndex)
    {
        InvenItem temp = invenItem[startIndex];
        invenItem[startIndex] = invenItem[targetIndex];
        invenItem[targetIndex] = temp;

        refreshAllSlot();
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
