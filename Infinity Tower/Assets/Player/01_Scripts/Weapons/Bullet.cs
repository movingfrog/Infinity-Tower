using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 forwardValue;
    float damage;
    Action<GameObject> enchantAction;

    public int[] layer;
    public float bulletSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(IgnoreWall());
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

    public void Init(
        Vector3 position,
        Quaternion rotation,
        Vector2 value,
        float amount,
        Action<GameObject> Enchant
    )
    {
        transform.position = position;
        transform.rotation = rotation;
        forwardValue = value;
        damage = amount;
        enchantAction = Enchant;
    }

    private void FixedUpdate()
    {
        rigid.linearVelocity = bulletSpeed * forwardValue;
    }

    private bool layerCheck(int _layer)
    {
        foreach (int temp in layer)
            if (temp == _layer)
                return true;
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layerCheck(collision.gameObject.layer))
        {
            if (collision.TryGetComponent<IHealth>(out IHealth health))
            {
                health.Hurt(damage);
                enchantAction.Invoke(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
