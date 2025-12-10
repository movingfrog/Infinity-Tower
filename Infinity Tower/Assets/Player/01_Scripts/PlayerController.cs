using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
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
    [Header("private형식의 접근 변수")]
    Rigidbody2D rigid;
    Animator ani;
    Vector2 movement;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
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
        ani.SetFloat("Velo", rigid.linearVelocityY);
        return hit.collider != null;
    }
    void movePosition()
    {
        float moveX = movement.x * basicMoveSpeed;
        if (movement.x != 0)
        {
            transform.localScale = new Vector2(movement.x, 1);
        }
        rigid.linearVelocityX = moveX;
    }

    public void AniDashControll()
    {
        ani.SetBool("isDash", false);
    } //애니메이션 이벤트 전용 메서드

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
    public void OnDash()
    {
        ani.SetBool("isDash", true);
        ani.Play("Dash", 0, 0);
        rigid.AddForceX(transform.localScale.x * 3, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, Vector2.down * groundDistance);
        Gizmos.DrawRay(ray);
    }
}
