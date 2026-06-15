using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionSlot : MonoBehaviour, IPointerClickHandler
{
    private Potion PotionSO;

    [Header("Slot 加己")]
    public int a;

    private Action<Potion> LeftClickAction;
    private Action<Potion> BuyAction;

    [Header("UI 加己")]
    public Image SlotIcon;
    public TextMeshProUGUI PotionName;
    public TextMeshProUGUI PotionPrice;

    private void Start()
    {
        SlotIcon.sprite = PotionSO.PotionIcon;
        PotionName.text = PotionSO.potionName;
        PotionPrice.text = PotionSO.price + "G";
    }

    public void SetUp(Potion _potion, Action<Potion> rightAction, Action<Potion> leftAction)
    {
        PotionSO = _potion;
        LeftClickAction = leftAction;
        BuyAction = rightAction;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClickAction.Invoke(PotionSO);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            BuyAction.Invoke(PotionSO);
        }
    }
}
