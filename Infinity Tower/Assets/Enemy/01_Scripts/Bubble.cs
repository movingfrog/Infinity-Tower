using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bubble : MonoBehaviour
{
    [SerializeField]
    private LayerMask Player;

    [SerializeField]
    private LayerMask Ground;

    [Space(10f)]
    [SerializeField]
    private float downForce = 7.5f;

    [Space(10f)]
    [SerializeField]
    private GameObject TrapBubblePrefab;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().AddForceY(-1 * downForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Player & (1 << collision.gameObject.layer)) != 0)
        {
            TrapBubble _TrapBubble = Instantiate(TrapBubblePrefab).GetComponent<TrapBubble>();
            _TrapBubble.transform.position = transform.position;
            _TrapBubble.SetUp(collision.transform);
            Destroy(transform.parent.gameObject);
        }
        else if ((Ground & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
