using UnityEngine;

public abstract class InvenParent : MonoBehaviour
{
    public abstract RectTransform CanvasTransform();
    public abstract void swapItem(int startIndex, int targetIndex);
}
