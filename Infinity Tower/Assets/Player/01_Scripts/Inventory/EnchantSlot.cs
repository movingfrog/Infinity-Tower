using UnityEngine;
using UnityEngine.UI;

public class EnchantSlot : MonoBehaviour
{
    private Button button;

    [Header("각인 UI연결 클래스")]
    public EnchantInfoUI EIUI;
    public EnchantInven EnchantInven;

    [Header("Slot 속성")]
    public Image SlotSprite;

    public int slotIndex;

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
}
