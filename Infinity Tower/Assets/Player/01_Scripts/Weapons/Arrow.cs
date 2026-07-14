using System;
using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float Damage;
    Rigidbody2D rigid;
    Action<GameObject> EnchantAction;

    public int[] layer;
    public float currentSpeed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
    }

    private void FixedUpdate()
    {
        if (rigid != null && rigid.linearVelocity.sqrMagnitude > .1f)
        {
            float angle = Mathf.Atan2(rigid.linearVelocityY, rigid.linearVelocityX) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
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

    public void Shot(Vector2 value, float percent, float damage, Action<GameObject> Enchant)
    {
        Damage = damage;
        rigid.linearVelocity = value * currentSpeed * percent;
        GetComponent<Collider2D>().enabled = true;
        EnchantAction = Enchant;
        StartCoroutine(IgnoreWall());
        StartCoroutine(useGravity(percent));
    }

    IEnumerator useGravity(float percent)
    {
        float flyTime = .3f + (percent * .7f);

        yield return new WaitForSeconds(flyTime);
        rigid.gravityScale = 1;
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
                health.Hurt(Damage);
                EnchantAction?.Invoke(collision.gameObject);
            }
            Destroy(GetComponent<Collider2D>());
            Destroy(rigid);
            transform.SetParent(collision.transform, true);
            Destroy(gameObject, 5f);
        }
    }
}
