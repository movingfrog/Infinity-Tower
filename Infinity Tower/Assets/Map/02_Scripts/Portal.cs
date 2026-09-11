using Unity.Cinemachine;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("애니메이션 제어")]
    Animator ani;
    public float R = 9.5f;
    public LayerMask Player;

    [Header("이동 관련")]
    public Portal arrivePos;
    public bool isTeleport;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, R, Player);
        ani.SetBool("isIn", player != null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTeleport && collision.CompareTag("Player"))
        {
            arrivePos.TpPlayer(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTeleport = false;
        }
    }

    protected virtual void TpPlayer(Collider2D player)
    {
        isTeleport = true;
        Vector3 oldPlayerPos = player.transform.position; // 텔레포트 전 위치 저장
        player.transform.position = transform.position; // 텔레포트

        var confinerChanger = player.GetComponent<CamConfinerChanger>();
        confinerChanger?.RecheckConfinerImmediate();

        CinemachineCamera vcam =
            GameManager.Instance.confiner.gameObject.GetComponent<CinemachineCamera>();
        CinemachineFollow follow = vcam.GetComponent<CinemachineFollow>();

        Vector3 targetCamPos = (Vector3)transform.position + follow.FollowOffset;

        // 핵심 수정: 플레이어가 "실제로 이동한 거리"를 delta로 전달
        Vector3 delta = player.transform.position - oldPlayerPos;

        vcam.PreviousStateIsValid = false;
        vcam.OnTargetObjectWarped(vcam.Follow, delta);
        vcam.ForceCameraPosition(targetCamPos, vcam.transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, R);
    }
}
