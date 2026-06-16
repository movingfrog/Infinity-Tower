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

    private const float groundCheck = .1f;
    private const float bounceCheck = 3f;

    [Range(0, 2)]
    public int jumpCount = 2;

    [Header("점프 판정 관련")]
    public float groundDistance;
    public LayerMask groundLayer;

    [Header("대쉬 관련")]
    public float dashForce;

    [Range(0, 2)]
    public int dashCount = 2;
    public bool isDashing;
    private int currentLayer;

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
        currentLayer = gameObject.layer;
    }

    private void FixedUpdate()
    {
        movePosition();
        if (isGrounded())
        {
            if (rigid.linearVelocityY <= groundCheck)
                jumpCount = 2;
            else if (rigid.linearVelocityY <= bounceCheck)
                rigid.linearVelocityY = 0;
        }
    }

    bool isGrounded()
    {
        if (ani.GetBool("isUsingSkill"))
            return false;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundDistance,
            groundLayer
        );
        ani.SetFloat("Velo", rigid.linearVelocityY);
        return hit.collider != null;
    }

    void movePosition()
    {
        if (!isDashing && !ani.GetBool("isUsingSkill"))
        {
            float moveX = movement.x * basicMoveSpeed;

            //if (rigid.linearVelocityX > moveX) return;
            rigid.linearVelocityX = moveX;
        }
    }

    public void AniDashControll()
    {
        ani.SetBool("isDash", false);
        isDashing = false;
        gameObject.layer = currentLayer;
        rigid.gravityScale = defaultGravity;
        dashCool = StartCoroutine(DashCool());
    } //애니메이션 이벤트 전용 메서드

    public void OnMove(InputValue value)
    {
        if (
            !PlayerStatManager.instance.getState(PlayerState.InvenOpen)
            && !PlayerStatManager.instance.getState(PlayerState.Idle)
        )
        {
            if (PlayerStatManager.instance.getState(PlayerState.Interacting))
            {
                NPCUI.instance.OnSelect(value);
            }
        }
        else
        {
            movement = value.Get<Vector2>();
            if (movement.x != 0)
                transform.localScale = new Vector2(Mathf.Sign(movement.x), 1);
        }
    }

    public void OnJump()
    {
        if (
            !PlayerStatManager.instance.getState(PlayerState.InvenOpen)
            && !PlayerStatManager.instance.getState(PlayerState.Idle)
        )
            return;
        if (jumpCount != 0 && !ani.GetBool("isUsingSkill"))
        {
            if (isDashing)
                rigid.gravityScale = defaultGravity;
            rigid.linearVelocityY = 0;
            rigid.AddForceY(JumpForce, ForceMode2D.Impulse);
            // WorkerHub<SoundWorker>.Instance.PlaySFX(GameManager.Instance.source, GameManager.Instance.SFX[0]); 예시 코드
            WorkerHub<SoundWorker>.Instance.PlaySFX(GameManager.Instance.Source, GameManager.Instance.SFX[0]);
            jumpCount--;
        }
    }

    public void OnDash()
    {
        if (
            !PlayerStatManager.instance.getState(PlayerState.InvenOpen)
            && !PlayerStatManager.instance.getState(PlayerState.Idle)
        )
            return;
        if (!isDashing && dashCount > 0 && !ani.GetBool("isUsingSkill"))
        {
            if (dashCool != null)
                StopCoroutine(dashCool);
            dashCount--;
            isDashing = true;
            ani.SetBool("isDash", true);
            ani.Play("Dash", 0, 0);
            WorkerHub<SoundWorker>.Instance.PlaySFX(GameManager.Instance.Source, GameManager.Instance.SFX[1]);
            gameObject.layer = 8;
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
