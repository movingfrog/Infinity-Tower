using TMPro;
using UnityEngine;

public class InteractionItem : DropItem
{
    private Color[] rarityColor = { Color.white, Color.cyan, Color.yellow };
    private GameObject InstItemInfo;

    private void Start()
    {
        InstItemInfo = SpaceUIManager.Instance.CreateItemUI(gameObject);
        TextMeshProUGUI TMP = InstItemInfo.GetComponentInChildren<TextMeshProUGUI>();
        TMP.color = rarityColor[(int)item.level];
        TMP.text = item.itemName;
        InstItemInfo.SetActive(false);
    }

    protected override void getItem()
    {
        InstItemInfo.SetActive(true);
        if (Input.GetKeyDown(KeyCode.F))
        {
            InventoryManager.Instance.GetItem(item, 1);
            Destroy(gameObject);
        }
    }

    protected override void moveItem(Collider2D outPlayer)
    {
        InstItemInfo.SetActive(false);
        base.moveItem(outPlayer);
    }
    private void OnDestroy()
    {
        if (InventoryManager.Instance != null && InstItemInfo != null)
        {
            SpaceUIManager.Instance.RemoveItemUI(InstItemInfo, gameObject);
        }
    }
}
