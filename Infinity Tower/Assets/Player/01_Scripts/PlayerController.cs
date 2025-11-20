using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("플레이어 물리 작용 관련")]
    public float basicMoveSpeed;
    public float basicJumpForce;
    [Space()]
    Rigidbody2D rigid;
    Vector2 movement;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveX = movement.x * basicMoveSpeed;

    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
}
