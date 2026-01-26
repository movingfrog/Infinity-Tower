using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    float Damage;
    Rigidbody2D rigid;

    public int[] layer;
    public float currentSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
    }

    public void Init(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    IEnumerator IgnoreWall()
    {
        int bulletLayer = gameObject.layer;
        int wallLayerIndex = LayerMask.NameToLayer("Wall");
        int groundLayerIndex = LayerMask.NameToLayer("Ground");

        Physics2D.IgnoreLayerCollision(bulletLayer, wallLayerIndex, true);
        Physics2D.IgnoreLayerCollision(bulletLayer, groundLayerIndex, true);

        yield return new WaitForSeconds(.05f);

        Physics2D.IgnoreLayerCollision(bulletLayer, wallLayerIndex, false);
        Physics2D.IgnoreLayerCollision(bulletLayer, groundLayerIndex, false);
    }
    public void Shot(Vector2 value, float percent, float damage)
    {
        Damage = damage;
        rigid.linearVelocity = value * currentSpeed * percent;
        StartCoroutine(IgnoreWall());
        StartCoroutine(useGravity(percent));
    }
    IEnumerator useGravity(float percent)
    {
        yield return new WaitForSeconds(currentSpeed * percent);
        rigid.gravityScale = 1;
    }
    private bool layerCheck(int _layer)
    {
        foreach (int temp in layer) if (temp == _layer) return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layerCheck(collision.gameObject.layer))
        {
            if(collision.TryGetComponent<IHealth>(out IHealth health))
            {
                health.Hurt(Damage);
            }
            rigid.linearVelocity = Vector2.zero;
            transform.parent = collision.transform;
            Destroy(gameObject, 5f);
        }
    }
}
