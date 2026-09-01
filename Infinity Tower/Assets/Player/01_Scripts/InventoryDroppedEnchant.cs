using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDroppedEnchant : DroppedEnchant
{
    protected override void OnInteract()
    {
        if (InventoryManager.Instance.EnchantInven.AddEnchant(enchant))
        {
            Destroy(gameObject);
        }
    }
}
