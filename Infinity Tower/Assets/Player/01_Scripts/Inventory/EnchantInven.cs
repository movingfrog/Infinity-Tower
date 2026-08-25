using UnityEngine;

public class EnchantInven : InvenParent
{
    public EnchantSlot[] allSlot;
    public WeaponEnchant[] allWeaponEnchant;

    public override bool canPlace(int targetIndex, InvenItem draggingItem)
    {
        return true;
    } // 사용 안함

    public override RectTransform CanvasTransform() => GetComponentInChildren<RectTransform>();

    public override void DropSlotItem(int slotNum) { } // 구현 계획 필요

    private void OnEnable()
    {
        RefreshAllSlot();
    }

    public override void RefreshAllSlot()
    {
        for (int i = 0; i < allSlot.Length; i++)
        {
            allSlot[i].refrashUI(allWeaponEnchant[i]);
        }
    }

    public bool AddEnchant(WeaponEnchant newEnchant)
    {
        for (int i = 0; i < allWeaponEnchant.Length; i++)
        {
            if (allWeaponEnchant[i] == null)
            {
                allWeaponEnchant[i] = newEnchant;
                RefreshAllSlot();
                return true;
            }
        }
        return false;
    }

    public override void swapItem(int startIndex, int targetIndex)
    {
        (allSlot[startIndex], allSlot[targetIndex]) = (allSlot[targetIndex], allSlot[startIndex]);

        RefreshAllSlot();
    }
}
