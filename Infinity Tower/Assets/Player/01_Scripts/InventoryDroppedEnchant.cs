using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDroppedEnchant : DroppedEnchant
{
    protected override void OnInteract(InputAction.CallbackContext context)
    {
        if (InventoryManager.Instance.EnchantInven.AddEnchant(enchant))
        {
            Destroy(gameObject);
        }
    }
}
