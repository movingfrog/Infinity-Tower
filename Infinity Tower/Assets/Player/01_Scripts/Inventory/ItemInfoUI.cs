using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public static ItemInfoUI Instance;

    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI itemStat;
    public Color statColor;
    public Color levelColor;
    public TextMeshProUGUI[] weaponMagicSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void drawText(Item item)
    {
        itemImage.sprite = item.spriteImage;
        itemName.text = item.itemName;
        itemInfo.text = item.itemInfo;
        //itemStat.text = item.
    }

    private void OnDestroy()
    {
        if(Instance == this) Instance = null;
    }
}
