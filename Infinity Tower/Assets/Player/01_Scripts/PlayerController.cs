using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("플레이어 물리 작용 관련")]
    public float basicMoveSpeed;
    public float JumpForce;
    [Range(0, 2)]
    public int jumpCount;
    [Header("점프 판정 관련")]
    public float groundDistance;
    public LayerMask groundLayer;
    [Space()]
    Rigidbody2D rigid;
    Vector2 movement;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        movePosition();
        if (isGrounded() && rigid.linearVelocityY == 0) jumpCount = 2;
    }

    bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundLayer);
        return hit.collider != null;
    }
    void movePosition()
    {
        float moveX = movement.x * basicMoveSpeed;
        if (movement.x != 0) transform.localScale = new Vector2(movement.x, 1);

        rigid.linearVelocityX = moveX;
    }

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if(jumpCount != 0)
        {
            rigid.linearVelocityY = 0;
            rigid.AddForceY(JumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, Vector2.down * groundDistance);
        Gizmos.DrawRay(ray);
    }
}
