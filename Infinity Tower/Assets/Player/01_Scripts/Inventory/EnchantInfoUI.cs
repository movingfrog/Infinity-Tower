using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnchantInfoUI : MonoBehaviour
{
    [Header("리셋 이미지")]
    public Sprite ResetImage;

    [Header("아이템 정보")]
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;

    private void OnEnable()
    {
        itemImage.sprite = ResetImage;
        itemName.text = "선택되지 않음";
        itemInfo.text = "선택된 인챈트가 없습니다. 획득한 각인을 선택해 주세요";
    }

    public void drawText(WeaponEnchant item)
    {
        itemImage.sprite = item.EnchantImage;
        itemName.text = item.EnchantName;
        itemInfo.text = item.EnchantExplain;
    }
}
