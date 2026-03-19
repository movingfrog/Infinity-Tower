using JetBrains.Annotations;
using UnityEditor.EventSystems;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public float moveSpeed;
    public float getSize;
    public float moveSize;
    public LayerMask player;

    private void FixedUpdate()
    {
        Collider2D outPlayer = Physics2D.OverlapCircle(transform.position, moveSize, player);

        if (outPlayer != null)
        {
            transform.position = Vector3.Lerp(transform.position, outPlayer.transform.position, moveSpeed * Time.deltaTime);
            if ((transform.position - outPlayer.transform.position).magnitude <= getSize)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, moveSize);
        Gizmos.DrawWireSphere(transform.position, getSize);
    }
}
