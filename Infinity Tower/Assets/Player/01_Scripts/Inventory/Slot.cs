using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Slot 加己")]
    private TextMeshProUGUI Text;
    public Image SlotSprite;
    public bool isEmpty = true;
    [Range(1, 9)]
    public int slotIndex;
    [Header("酒捞袍 加己")]
    public Item item;
    public int itemCount;

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
        Text.gameObject.SetActive(false);
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
                Text.gameObject.SetActive(true);
                Text.text = itemCount.ToString("0");
            }
        }
    }
    public void GetItemInfo()
    {

    }

    void ChangeSlot()
    {
        SlotSprite.sprite = item.spriteImage;
        SlotSprite.color = Color.white;
    }
}
