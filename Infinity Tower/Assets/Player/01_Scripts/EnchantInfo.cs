using TMPro;
using UnityEngine;

public class EnchantInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI EnchantName;

    [SerializeField]
    private TextMeshProUGUI EnchantExplain;

    public void SetInfo(WeaponEnchant enchant)
    {
        if (enchant != null)
        {
            EnchantName.text = enchant.EnchantName;
            EnchantExplain.text = enchant.EnchantExplain;
        }
    }
}
