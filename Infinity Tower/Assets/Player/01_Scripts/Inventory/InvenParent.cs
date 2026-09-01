using UnityEngine;

public abstract class InvenParent : MonoBehaviour
{
    public abstract RectTransform CanvasTransform();

    /// <summary>
    /// 인벤토리 내 아이템 위치 변경
    /// </summary>
    /// <param name="startIndex">바꿀 아이템</param>
    /// <param name="targetIndex">바뀔 위치</param>
    public abstract void swapItem(int startIndex, int targetIndex);
    public abstract void RefreshAllSlot();
    public abstract bool canPlace(int targetIndex, InvenItem draggingItem);
    public abstract void DropSlotItem(int slotNum);
}
