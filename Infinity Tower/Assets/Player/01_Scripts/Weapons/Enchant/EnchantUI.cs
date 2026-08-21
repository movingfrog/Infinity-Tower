using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponSlot
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponNameShadow;
    public TextMeshProUGUI FirstEnchantName;
    public TextMeshProUGUI SecondEnchantName;
    public TextMeshProUGUI FirstEnchantExplain;
    public TextMeshProUGUI SecondEnchantExplain;
    public Image BackImage;
    public Image weaponImage;
    public Image FirstEnchantImage;
    public Image SecondEnchantImage;

    public void SetWeaponInfo(InvenItem weaponItem)
    {
        if (
            weaponItem.item == null
            || (
                GameManager.Instance.allWeaponData.ContainsKey(weaponItem.weaponGuid)
                && !GameManager.Instance.allWeaponData[weaponItem.weaponGuid].CanEnchant()
            )
        )
            BackImage.color = Color.red;
        else
            BackImage.color = Color.lightGray;
        if (weaponItem.item != null)
        {
            weaponName.text = weaponItem.item.itemName;
            weaponNameShadow.text = weaponItem.item.itemName;
            weaponImage.sprite = weaponItem.item.spriteImage;
            if (GameManager.Instance.allWeaponData[weaponItem.weaponGuid].enchants[0] != null)
            {
                FirstEnchantName.text = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[0]
                    .EnchantName;
                FirstEnchantExplain.text = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[0]
                    .EnchantExplain;
                FirstEnchantImage.sprite = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[0]
                    .EnchantImage;
            }
            else
            {
                FirstEnchantName.text = "없음";
                FirstEnchantExplain.text = "";
                FirstEnchantImage.sprite = null;
            }
            if (GameManager.Instance.allWeaponData[weaponItem.weaponGuid].enchants[1] != null)
            {
                SecondEnchantName.text = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[1]
                    .EnchantName;
                SecondEnchantExplain.text = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[1]
                    .EnchantExplain;
                SecondEnchantImage.sprite = GameManager
                    .Instance
                    .allWeaponData[weaponItem.weaponGuid]
                    .enchants[1]
                    .EnchantImage;
            }
            else
            {
                SecondEnchantName.text = "없음";
                SecondEnchantExplain.text = "";
                SecondEnchantImage.sprite = null;
            }
        }
        else
        {
            weaponName.text = "없음";
            weaponNameShadow.text = "없음";
            weaponImage.sprite = null;
            FirstEnchantName.text = "없음";
            FirstEnchantExplain.text = "";
            FirstEnchantImage.sprite = null;
            SecondEnchantName.text = "없음";
            SecondEnchantExplain.text = "";
            SecondEnchantImage.sprite = null;
        }
    }
}

public class EnchantUI : MonoBehaviour
{
    private InvenItem FirstWeaponItem;
    private InvenItem SecondWeaponItem;

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
        (FirstWeaponItem, SecondWeaponItem) = InventoryManager.Instance.GetEquipWeaponItem();
        EnchantImage.sprite = enchant.EnchantImage;
        EnchantName.text = enchant.EnchantName;
        EnchantExplain.text = enchant.EnchantExplain;
        FirstWeaponSlot.SetWeaponInfo(FirstWeaponItem);
        SecondWeaponSlot.SetWeaponInfo(SecondWeaponItem);
    }

    public void EnchnatWeaponFirst()
    {
        if (
            FirstWeaponItem != null
            && GameManager.Instance.allWeaponData[FirstWeaponItem.weaponGuid].CanEnchant()
        )
        {
            GameManager.Instance.allWeaponData[FirstWeaponItem.weaponGuid].EquipEnchant(enchant);
            Time.timeScale = 1f; // 게임 재게
            gameObject.SetActive(false); // UI 비활성화
            Debug.Log("첫번째 무기 각인 장착");
        }
    }

    public void EnchantWeaponSecond()
    {
        if (
            SecondWeaponItem != null
            && GameManager.Instance.allWeaponData[SecondWeaponItem.weaponGuid].CanEnchant()
        )
        {
            GameManager.Instance.allWeaponData[SecondWeaponItem.weaponGuid].EquipEnchant(enchant);
            Time.timeScale = 1f; // 게임 재게
            gameObject.SetActive(false); // UI 비활성화
            Debug.Log("두번째 무기 각인 장착");
        }
    }

    public void DropEnchantItem()
    {
        Debug.Log("각인 버리기");
        Time.timeScale = 1f; // 게임 재게
        gameObject.SetActive(false); // UI 비활성화
    }

    public void SaveEnchantItem()
    {
        Debug.Log("각인 저장");
        Time.timeScale = 1f; // 게임 재게
        gameObject.SetActive(false); // UI 비활성화
    }
}
