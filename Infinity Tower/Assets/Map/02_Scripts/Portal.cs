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
        Vector3 playerPos = player.transform.position;
        player.transform.position = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, R);
    }
}
