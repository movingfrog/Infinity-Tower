using UnityEngine;
using UnityEngine.UI;

public class AnvilEnchnatSlot : MonoBehaviour
{
    [SerializeField]
    private BlackSmithSystem BSS;

    EnchantSlot slot;

    private void Awake()
    {
        slot = GetComponent<EnchantSlot>();
        GetComponent<Button>().onClick.AddListener(ShowInfo);
    }

    private void ShowInfo()
    {
        if (slot != null)
        {
            if (slot.EnchantInven.allWeaponEnchant[slot.slotIndex] != null)
            {
                BSS.OpenEnchantInfo();
            }
        }
    }
}
