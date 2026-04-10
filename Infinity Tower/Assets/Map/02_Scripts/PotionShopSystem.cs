using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionShopSystem : MonoBehaviour
{
    [Header("물약 데이터")]
    public Potion[] Potions;
    public PotionSlot[] Slots;

    [Header("UI 속성")]
    public TextMeshProUGUI PotionName;
    public TextMeshProUGUI PotionInfo;
    public Image PotionIcon;

    private void Awake()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].SetUp(Potions[i], BuyPotion, ShowInfo);
        }
    }

    void ShowInfo(Potion _potion)
    {
        PotionName.text = _potion.potionName;
        PotionInfo.text = _potion.potionInfo;
        PotionIcon.sprite = _potion.PotionIcon;
    }

    void BuyPotion(Potion _potion) { }
}
