using UnityEngine;
using UnityEngine.InputSystem;

public class TrapBubble : MonoBehaviour
{
    Transform PlayerObject;
    IHealth PlayerHP;

    [SerializeField]
    private int TrappingCount;

    [SerializeField]
    private float Damage;

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.BreakBubble.started += BreakBubble;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.BreakBubble.started -= BreakBubble;
    }

    public void SetUp(Transform Player)
    {
        PlayerObject = Player;
        PlayerHP = Player.GetComponent<IHealth>();
    }

    private void BreakBubble(InputAction.CallbackContext context)
    {
        if (TrappingCount > 0)
        {
            PlayerHP.Hurt(Damage);
            TrappingCount--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        PlayerObject.position = transform.position;
    }
}
