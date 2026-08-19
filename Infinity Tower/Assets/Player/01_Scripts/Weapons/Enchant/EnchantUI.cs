using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WeaponSlot
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI FirstEnchantName;
    public TextMeshProUGUI SecondEnchantName;
    public TextMeshProUGUI FirstEnchantExplain;
    public TextMeshProUGUI SecondEnchantExplain;
    public Image weaponImage;
    public Image FirstEnchantImage;
    public Image SecondEnchantImage;

    public void SetWeaponInfo(Weapon weapon, Item weaponItem)
    {
        if (weapon != null)
        {
            weaponName.text = weaponItem.itemName;
            weaponImage.sprite = weaponItem.spriteImage;
            if (weapon.enchants[0] != null)
            {
                FirstEnchantName.text = weapon.enchants[0].EnchantName;
                FirstEnchantExplain.text = weapon.enchants[0].EnchantExplain;
                FirstEnchantImage.sprite = weapon.enchants[0].EnchantImage;
            }
            else
            {
                FirstEnchantName.text = "없음";
                FirstEnchantExplain.text = "";
                FirstEnchantImage.sprite = null;
            }
            if (weapon.enchants[1] != null)
            {
                SecondEnchantName.text = weapon.enchants[1].EnchantName;
                SecondEnchantExplain.text = weapon.enchants[1].EnchantExplain;
                SecondEnchantImage.sprite = weapon.enchants[1].EnchantImage;
            }
            else
            {
                SecondEnchantName.text = "없음";
                SecondEnchantExplain.text = "";
                SecondEnchantImage.sprite = null;
            }
        }
    }
}

public class EnchantUI : MonoBehaviour
{
    public WeaponEnchant enchant { get; set; }

    [Header("장착 무기")]
    [SerializeField]
    private WeaponSlot FirstWeaponSlot;

    [SerializeField]
    private WeaponSlot SecondWeaponSlot;

    [Header("각인 설명")]
    [SerializeField]
    private Image EnchantImage;

    [SerializeField]
    private TextMeshProUGUI EnchantName;

    [SerializeField]
    private TextMeshProUGUI EnchantExplain;

    public void SetAll(WeaponEnchant _Enchant)
    {
        enchant = _Enchant;
        EnchantImage.sprite = enchant.EnchantImage;
        EnchantName.text = enchant.EnchantName;
        EnchantExplain.text = enchant.EnchantExplain;
        //FirstWeaponSlot.SetWeaponInfo()
    }
}
