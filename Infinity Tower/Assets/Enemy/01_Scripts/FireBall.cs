using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float moveSpeed;
    Vector2 angle;
    float Damage;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if (collision.TryGetComponent<IHealth>(out IHealth Phealth))
            {
                Phealth.Hurt(Damage);
            }
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 _angle, float _damage)
    {
        angle = _angle;
        Damage = _damage;

        Shot();
    }

    void Shot()
    {
        rigid.linearVelocity = angle * moveSpeed;
    }
}
