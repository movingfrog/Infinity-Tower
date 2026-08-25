using UnityEngine;
using UnityEngine.InputSystem;

public class ChastDroppedEnchant : DroppedEnchant
{
    [SerializeField]
    private EnchantUI EnchantCanvas;

    protected override void OnInteract(InputAction.CallbackContext context)
    {
        if (InfoObject.gameObject.activeSelf)
        {
            EnchantCanvas.gameObject.SetActive(true);
            EnchantCanvas.SetAll(enchant);
            Time.timeScale = 0f; // 게임 일시정지
            Debug.Log(Time.timeScale);
        }
    }
}
