using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnchantSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Button button;

    [Header("각인 UI연결 클래스")]
    public EnchantInfoUI EIUI;
    public EnchantInven EnchantInven;

    [Header("Slot 속성")]
    public Image SlotSprite;

    public int slotIndex;

    [Header("Drag 속성")]
    Transform dragAfterParent;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GetItemInfo);
    }

    public void refrashUI(WeaponEnchant Enchant)
    {
        if (Enchant != null)
        {
            SlotSprite.sprite = Enchant.EnchantImage;
            SlotSprite.color = Color.white;
        }
        else
        {
            SlotSprite.sprite = null;
            SlotSprite.color = Color.white * 0;
        }
    }

    public void GetItemInfo()
    {
        if (EnchantInven.allWeaponEnchant[slotIndex] == null)
            return;
        EIUI.drawText(EnchantInven.allWeaponEnchant[slotIndex]);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SlotSprite.rectTransform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragAfterParent = SlotSprite.rectTransform.parent;
        SlotSprite.rectTransform.SetParent(EnchantInven.CanvasTransform());
        SlotSprite.transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (
            eventData.pointerCurrentRaycast.gameObject != null
            && eventData.pointerCurrentRaycast.gameObject.TryGetComponent(
                out EnchantSlot targetSlot
            )
        )
        {
            EnchantInven.swapItem(this.slotIndex, targetSlot.slotIndex);
        }
        else if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            EnchantInven.DropSlotItem(slotIndex);
        }
        SlotSprite.rectTransform.SetParent(dragAfterParent);
    }
}
