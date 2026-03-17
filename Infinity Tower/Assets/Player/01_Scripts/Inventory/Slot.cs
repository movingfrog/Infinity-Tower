using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType { Inventory, Weapon, Accessory, Import }

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TextMeshProUGUI Text;
    private Button button;
    [Header("Drag 加己")]
    public bool hasDrag;
    Transform dragAfterParent;
    [Header("Slot 加己")]
    public SlotType type;
    public Image SlotSprite;
    [Range(0, 8)]
    public int slotIndex;
    [Foldout("抗寇 浇吩")]
    public Sprite defaultSprite;

    private void Awake()
    {
        if (type == SlotType.Inventory)
        {
            Text = GetComponentInChildren<TextMeshProUGUI>();
            Text.color = new Color(0, 0, 0, 0);
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(GetItemInfo);
    }

    public void refrashUI(Item currentItem, int currentItemCount = 0)
    {
        if(currentItem != null)
        {
            SlotSprite.sprite = currentItem.spriteImage;
            SlotSprite.color = Color.white;
            if (!currentItem.isWearable)
            {
                Text.color = Color.white;
                Text.text = currentItemCount.ToString("0");
            }
        }
        else
        {
            if(type == SlotType.Inventory)
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
        if (InventoryManager.Instance.invenItem[slotIndex].item == null) return;
        ItemInfoUI.Instance.OpenInfo(InventoryManager.Instance.invenItem[slotIndex].item);
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
        SlotSprite.rectTransform.SetParent(InventoryManager.Instance.GetComponentInChildren<RectTransform>());
        SlotSprite.transform.SetAsLastSibling();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!hasDrag) return;
        if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out Slot targetSlot))
        {
            InventoryManager.Instance.swapItem(this.slotIndex, targetSlot.slotIndex, targetSlot.type, type);
        }

        SlotSprite.rectTransform.SetParent(dragAfterParent);
    }
}
    