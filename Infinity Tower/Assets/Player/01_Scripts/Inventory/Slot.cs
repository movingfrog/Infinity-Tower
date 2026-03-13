using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Slot 加己")]
    private TextMeshProUGUI Text;
    public Image SlotSprite;
    public bool isEmpty = true;
    [Range(0, 8)]
    public int slotIndex;
    [Header("酒捞袍 加己")]
    public Item item;
    public int itemCount;

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
        Text.color = new Color(0, 0, 0, 0);
    }

    public void GetItem(Item currentItem, int currentItemCount)
    {
        if (isEmpty)
        {
            isEmpty = false;
            item = currentItem;
            itemCount += currentItemCount;
            ChangeSlot();
        }
        else
        {
            if(item == currentItem && item.MaxItemCount < itemCount + currentItemCount)
            {
                itemCount += currentItemCount;
                Text.color = Color.white;
                Text.text = itemCount.ToString("0");
            }
        }
    }
    public void GetItemInfo()
    {
        if (item == null) return;
        ItemInfoUI.Instance.OpenInfo(item);
    }

    void ChangeSlot()
    {
        SlotSprite.sprite = item.spriteImage;
        SlotSprite.color = Color.white;
    }
}
