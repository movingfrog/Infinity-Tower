using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossChestEnchantSpawn : MonoBehaviour
{
    private Chest BossChest;

    [Space(10f), SerializeField]
    private WeaponEnchant enchant;

    [SerializeField, Foldout("랜덤 각인 시 사용")]
    private int maxEnchantLevel;

    private void Awake()
    {
        BossChest = GetComponent<Chest>();
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.inputActions.Player.Interact.started += Open;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.inputActions.Player.Interact.started -= Open;
    }

    private void Open(InputAction.CallbackContext callbackContext)
    {
        if (BossChest != null && BossChest.IsIn)
        {
            if (enchant == null)
                enchant = WorkerHub<GetRandomEnchant>.Instance.RandEnchantWorker(
                    GameManager.Instance.allEnchant,
                    maxEnchantLevel
                );
            Instantiate(
                    GameManager.Instance.ChestDropEnchantObject,
                    transform.position,
                    Quaternion.identity
                )
                .GetComponent<ChastDroppedEnchant>()
                .enchant = enchant;
            Destroy(this);
        }
    }
}
