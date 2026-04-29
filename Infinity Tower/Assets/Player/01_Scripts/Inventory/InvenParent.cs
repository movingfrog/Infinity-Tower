using UnityEngine;

public abstract class InvenParent : MonoBehaviour
{
    public abstract RectTransform CanvasTransform();
    public abstract void swapItem(int startIndex, int targetIndex);
    public abstract void RefreshAllSlot();
    public abstract bool canPlace(int targetIndex, InvenItem draggingItem);
}
