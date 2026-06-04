using UnityEngine;

public class LootGoods : MonoBehaviour
{
    [SerializeField]
    private GoodsType Type;

    [SerializeField]
    private uint amount;

    [Header("물리 작용")]
    [SerializeField]
    private float XForce;

    [SerializeField]
    private float YForce;

    [SerializeField]
    private LayerMask Layer;

    private void Awake()
    {
        float randXforce = Random.Range(-XForce, XForce);
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
        {
            rigid.AddForce(new Vector2(randXforce, YForce), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Layer & (1 << collision.gameObject.layer)) != 0)
        {
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.GetGoods(Type, amount);
            gameObject.SetActive(false);
        }
    }
}
