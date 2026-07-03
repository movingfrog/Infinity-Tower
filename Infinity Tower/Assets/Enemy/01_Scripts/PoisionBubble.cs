using System.Collections;
using UnityEngine;

public class PoisionBubble : MonoBehaviour
{
    [SerializeField]
    private int PoisionTickAmount;

    [SerializeField]
    private float AttackDamage;

    [Space(10f), SerializeField]
    private LayerMask Player;

    [SerializeField]
    private LayerMask Ground;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Player & (1 << collision.gameObject.layer)) != 0)
        {
            StartCoroutine(PoisionDotDamage(collision));
            Destroy(gameObject);
        }
        if ((Ground & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator PoisionDotDamage(Collider2D Player)
    {
        IHealth PlayerHealth = Player.GetComponent<IHealth>();
        for (int i = 0; i < PoisionTickAmount; i++)
        {
            PlayerHealth.Hurt(AttackDamage);
            yield return new WaitForSeconds(1f);
        }
    }
}
