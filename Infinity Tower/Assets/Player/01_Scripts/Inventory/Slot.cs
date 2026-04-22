using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    Inventory,
    Weapon,
    Accessories,
    Import,
    Anvil,
}

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TextMeshProUGUI Text;
    private Button button;

    [Header("관리자")]
    public InvenParent invenManager;

    [Header("Drag 속성")]
    public bool hasDrag;
    Transform dragAfterParent;

    [Header("Slot 속성")]
    public SlotType type;
    public Image SlotSprite;

    public int slotIndex;
    public bool isStoreUI;

    [Foldout("예외 슬롯")]
    public Sprite defaultSprite;

    private void Awake()
    {
        if (type == SlotType.Inventory)
        {
            Text = GetComponentInChildren<TextMeshProUGUI>();
            Text.color = new Color(0, 0, 0, 0);
        }
        if (!isStoreUI)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(GetItemInfo);
        }
    }

    public void refrashUI(InvenItem currentItem)
    {
        if (currentItem.item != null)
        {
            SlotSprite.sprite = currentItem.item.spriteImage;
            SlotSprite.color = Color.white;
            if (!currentItem.item.isEquippable)
            {
                Text.color = Color.white;
                Text.text = currentItem.currentItemCount.ToString("0");
            }
        }
        else
        {
            if (type == SlotType.Inventory)
            {
                SlotSprite.sprite = null;
                SlotSprite.color = Color.white * 0;
                Text.color = new Color(0, 0, 0, 0);
            }
            else
            {
                SlotSprite.sprite = defaultSprite;
                SlotSprite.color = new Color(1, 1, 1, .25f);
            }
        }
    }

    public void GetItemInfo()
    {
        if (InventoryManager.Instance.allItem[slotIndex].item == null)
            return;
        ItemInfoUI.Instance.OpenInfo(InventoryManager.Instance.allItem[slotIndex].item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!hasDrag)
            return;
        SlotSprite.rectTransform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasDrag)
            return;
        dragAfterParent = SlotSprite.rectTransform.parent;
        SlotSprite.rectTransform.SetParent(invenManager.CanvasTransform());
        SlotSprite.transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!hasDrag)
            return;
        if (
            eventData.pointerCurrentRaycast.gameObject != null
            && eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out Slot targetSlot)
        )
        {
            invenManager.swapItem(this.slotIndex, targetSlot.slotIndex);
        }

        SlotSprite.rectTransform.SetParent(dragAfterParent);
    }
}
