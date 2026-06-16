using UnityEngine;

public abstract class DropItem : MonoBehaviour
{
    [Header("아이템 정보")]
    public Item item;

    [Header("아이템 획득")]
    public float moveSpeed = 1;
    public float getSize = .75f;
    public float moveSize = 1;
    public LayerMask player;

    protected virtual void Start()
    {
        if (item != null && TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.sprite = item.spriteImage;
        }
    }

    private void FixedUpdate()
    {
        Collider2D outPlayer = Physics2D.OverlapCircle(transform.position, moveSize, player);

        if (outPlayer != null)
        {
            if ((transform.position - outPlayer.transform.position).magnitude <= getSize)
            {
                getItem();
            }
            else
            {
                moveItem(outPlayer);
            }
        }
    }

    protected virtual void moveItem(Collider2D outPlayer)
    {
        transform.position = Vector3.Lerp(
            transform.position,
            outPlayer.transform.position,
            moveSpeed * Time.deltaTime
        );
    }

    protected abstract void getItem();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, moveSize);
        Gizmos.DrawWireSphere(transform.position, getSize);
    }
}
