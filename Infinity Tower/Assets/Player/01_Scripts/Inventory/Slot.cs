using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TextMeshProUGUI Text;
    private Button button;
    [Header("Drag 加己")]
    public bool hasDrag;
    Transform dragAfterParent;
    [Header("Slot 加己")]
    public Image SlotSprite;
    public bool isEmpty = true;
    [Range(0, 8)]
    public int slotIndex;
    [Header("酒捞袍 加己")]
    public Item item;
    public int itemCount;

    private void Awake()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        Text.color = new Color(0, 0, 0, 0);
        button.onClick.AddListener(GetItemInfo);
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

    public void OnDrag(PointerEventData eventData)
    {
        if (!hasDrag) return;
        SlotSprite.rectTransform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasDrag) return;
        dragAfterParent = SlotSprite.rectTransform.parent;
        SlotSprite.rectTransform.parent = transform.root;
        SlotSprite.transform.SetAsLastSibling();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!hasDrag) return;
        SlotSprite.rectTransform.SetParent(dragAfterParent);
        SlotSprite.rectTransform.position = Vector3.zero;
    }
}
    