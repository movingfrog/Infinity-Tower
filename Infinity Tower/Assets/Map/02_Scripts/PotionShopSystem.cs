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
    public TextMeshProUGUI SatietyText;
    public Image PotionIcon;
    public Image SatietyBarImage;

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
        RefreshSatietyBar();
    }

    void RefreshSatietyBar()
    {
        SatietyBarImage.fillAmount = PlayerStatManager.instance.Satiety / 100f;
        SatietyText.text = PlayerStatManager.instance.Satiety.ToString("0") + "/100";
    }

    //0을 potion의 price로 바꿀 필요 있음
    void BuyPotion(Potion _potion)
    {
        if (
            PlayerStatManager.instance.Satiety < 100
            && InventoryManager.Instance.UseGoods(GoodsType.Gold, 0)
        )
        {
            PlayerStatManager.instance.IncreassHealth(_potion.healthAmount);
            PlayerStatManager.instance.ChangeHealth(_potion.healAmount - _potion.healthAmount);
            PlayerStatManager.instance.Satiety += _potion.satietyAmount;
            RefreshSatietyBar();
        }
    }

    public void BackToGame()
    {
        PlayerStatManager.instance.resetState();
        gameObject.SetActive(false);
    }
}
