using UnityEngine;

public enum GoodsType
{
    Gold,
    Stone,
    AcientStone,
}

public class MagnetItem : DropItem
{
    public int Amount;

    protected override void getItem()
    {
        if (InventoryManager.Instance == null)
            return;
        InventoryManager.Instance.GetItem(item, Amount);
        //아이템이 남으면 넘어가는 예외처리 필요
        //획득하는 소리 추가 필요
        Debug.LogError("획득하는 소리 필요");
        Destroy(gameObject);
    }
}
