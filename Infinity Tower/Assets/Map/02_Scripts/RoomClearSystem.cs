using UnityEngine;
using UnityEngine.InputSystem;

public class RoomClearSystem : MonoBehaviour
{
    [SerializeField]
    private MoveRoomPortal InPortal;

    [SerializeField]
    private MoveRoomPortal OutPortal;

    [Space(10f), SerializeField]
    private RewardSystem RewardSystem;

    public bool Clear { get; private set; }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += OpenChest;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= OpenChest;
    }

    public void RoomClear()
    {
        InPortal.gameObject.SetActive(true);
        OutPortal.gameObject.SetActive(true);
    }

    private void OpenChest(InputAction.CallbackContext callback)
    {
        if (RewardSystem == null && !Clear)
        {
            RoomClear();
            Clear = true;
        }
    }
}
