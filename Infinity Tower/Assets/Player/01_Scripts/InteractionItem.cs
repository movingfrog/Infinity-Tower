using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionItem : DropItem
{
    private Color[] rarityColor = { Color.white, Color.cyan, Color.yellow };
    private GameObject InstItemInfo;

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += InteractGetItem;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= InteractGetItem;
    }

    private void Start()
    {
        InstItemInfo = SpaceUIManager.Instance.CreateItemUI(gameObject);
        TextMeshProUGUI TMP = InstItemInfo.GetComponentInChildren<TextMeshProUGUI>();
        TMP.color = rarityColor[(int)item.level];
        TMP.text = item.itemName;
        InstItemInfo.SetActive(false);
    }

    public void InteractGetItem(InputAction.CallbackContext callback)
    {
        InventoryManager.Instance.GetItem(item, 1);
        Destroy(gameObject);
    }

    protected override void getItem()
    {
        InstItemInfo.SetActive(true);
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
