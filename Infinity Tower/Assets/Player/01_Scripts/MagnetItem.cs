using UnityEngine;

public enum GoodsType
{
    Gold,
    Stone,
    AcientStone,
}

public class MagnetItem : DropItem
{
    public bool isInven;
    public int Amount;

    protected override void getItem()
    {
        if (isInven)
        {
            InventoryManager.Instance.GetItem(item, Amount);
            //È¹µæÇÏ´Â ¼Ò¸® Ãß°¡ ÇÊ¿ä
            Debug.LogError("È¹µæÇÏ´Â ¼Ò¸® ÇÊ¿ä");
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("¾ÆÁ÷ ±¸Çö ¾ÈµÊ");
            Destroy(gameObject);
        }
    }
}
