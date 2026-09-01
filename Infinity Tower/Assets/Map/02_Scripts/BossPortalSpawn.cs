using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossPortalSpawn : MonoBehaviour
{
    private Chest BossChest;

    [Space(10f), SerializeField]
    private GameObject Portal;

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
        if (BossChest != null && BossChest.InteractionObject.activeSelf)
        {
            Portal.SetActive(true);
        }
    }
}
