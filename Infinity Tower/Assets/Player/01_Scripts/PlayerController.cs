using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TimeBody))]
public class PlayerController : MonoBehaviour
{
    [Header("플레이어 물리 작용 관련")]
    public float basicMoveSpeed;
    public float JumpForce;
    [Range(0, 2)]
    public int jumpCount = 2;
    [Header("점프 판정 관련")]
    public float groundDistance;
    public LayerMask groundLayer;
    [Header("대쉬 관련")]
    public float dashForce;
    [Range(0,2)]
    public int dashCount = 2;
    public int dir;
    public bool isDashing;
    [Space]
    float defaultGravity;
    Coroutine dashCool;
    [Header("private형식의 접근 변수")]
    Rigidbody2D rigid;
    Animator ani;
    Vector2 movement;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
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
        if(!isDashing && ani.GetBool("isUsingSKill"))
        {
            float moveX = movement.x * basicMoveSpeed;
            if (movement.x != 0)
            {
                transform.localScale = new Vector2(movement.x, 1);
            }
            rigid.linearVelocityX = moveX;
        }
    }

    public void AniDashControll()
    {
        ani.SetBool("isDash", false);
        isDashing  = false;
        rigid.gravityScale = defaultGravity;
        dashCool = StartCoroutine(DashCool());
    } //애니메이션 이벤트 전용 메서드

    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        if (movement.x != 0) dir = (int)movement.x;
    }
    public void OnJump()
    {
        if(jumpCount != 0 && ani.GetBool("isUsingSKill"))
        {
            rigid.linearVelocityY = 0;
            rigid.AddForceY(JumpForce, ForceMode2D.Impulse);
            jumpCount--;
        }
    }
    public void OnDash()
    {
        if(!isDashing && dashCount > 0 && ani.GetBool("isUsingSKill"))
        {
            if(dashCool != null) StopCoroutine(dashCool);
            dashCount--;
            isDashing = true;
            ani.SetBool("isDash", true);
            ani.Play("Dash", 0, 0);
            defaultGravity = rigid.gravityScale;
            rigid.gravityScale = 0;
            rigid.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
        }
    }

    IEnumerator DashCool()
    {
        yield return new WaitForSeconds(1f);
        dashCount = 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Ray ray = new Ray(transform.position, Vector2.down * groundDistance);
        Gizmos.DrawRay(ray);
    }
}
