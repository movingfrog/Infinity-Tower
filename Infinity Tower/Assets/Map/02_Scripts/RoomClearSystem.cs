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

    [Header("플레이어 탐지 관련")]
    [SerializeField]
    private LayerMask PlayerLayer;

    [SerializeField]
    private float radius;
    public bool Clear { get; private set; }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.inputActions.Player.Interact.started += OpenChest;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
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
            Collider2D PlayerColl = Physics2D.OverlapCircle(
                transform.position,
                radius,
                PlayerLayer
            );
            if (PlayerColl != null)
            {
                RoomClear();
                Clear = true;
            }
        }
    }
}
