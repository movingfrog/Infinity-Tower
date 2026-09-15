using UnityEngine;

public class OnCrownTechPointer : OnTechPointer
{
    [SerializeField, Tooltip("능력 종류 개수")]
    private int UnlockAmount = 0;

    protected override void UnlockLevel()
    {
        if (UnlockAmount > 0)
            UnlockAmount--;
        else
        {
            BGImage.color = canPurchaseColor;
            isLock = false;
        }
    }

    public override void Purchase()
    {
        BGImage.color = purchaseColor;
    }
}
