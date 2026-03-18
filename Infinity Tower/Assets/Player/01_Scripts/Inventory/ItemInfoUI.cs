using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public static ItemInfoUI Instance;
    public static readonly string[] ItemLevelName = { "일반", "희귀", "전설" };
    public static readonly string[] ItemStatName = { "공격력", "치명타 확률", "치명타 피해", "이동 속도", "골드 획득량", "회복량" };
    public static readonly Color[] ItemLevelColor = { Color.white, Color.cyan, Color.yellow };

    [Header("이동 애니메이션")]
    public Vector3 endPos;
    public float duration;
    RectTransform detailPanel;
    private Vector3 startPos;
    [Header("아이템 정보")]
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI itemStat;
    public Color statColor;
    public TextMeshProUGUI[] weaponMagicSlot;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        detailPanel = GetComponent<RectTransform>();
        startPos = transform.position;
    }

    public void OpenInfo(Item item)
    {
        drawText(item);
        detailPanel.DOAnchorPos(endPos, duration).SetEase(Ease.OutBounce);
    }
    public void CloseInfo() => detailPanel.DOMove(startPos, duration).SetEase(Ease.OutCirc);

    public void drawText(Item item)
    {
        itemImage.sprite = item.spriteImage;
        itemName.text = item.itemName;
        itemInfo.text = item.itemInfo;
        string itemLevelHex = "#" + ColorUtility.ToHtmlStringRGB(ItemLevelColor[(int)item.level]);
        if (item.isEquippable)
        {
            itemStat.color = statColor;
            switch (item.slotType)
            {
                case SlotType.Weapon:
                    itemStat.text = $"{ItemStatName[0]}: +{item.Equips.itemDamage}";
                    break;
                case SlotType.Accessories:
                    itemStat.text = "";
                    for(int i = 0; i < item.Equips.statModifiers.Count; i++)
                    {
                        itemStat.text += $"{ItemStatName[(int)item.Equips.statModifiers[i].Type]}: " + (item.Equips.statModifiers[i].Type == StatType.ATK ? "+" : "")
                            + item.Equips.statModifiers[i].Value.ToString("0") + (item.Equips.statModifiers[i].Type != StatType.ATK ? "%" : "") + "\n";
                    }
                    break;
            }
        }
        itemStat.text += $"<color={itemLevelHex}>희귀도: {ItemLevelName[(int)item.level]}";
    }

    private void OnDestroy()
    {
        if(Instance == this) Instance = null;
    }
}
