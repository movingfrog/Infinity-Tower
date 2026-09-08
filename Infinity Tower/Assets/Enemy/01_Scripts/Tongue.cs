using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tongue : MonoBehaviour
{
    [SerializeField]
    private LayerMask Player;

    [SerializeField]
    private LayerMask Ground;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((Player & (1 << collision.gameObject.layer)) != 0)
        {
            PlayerStatManager.instance.StartCoroutine(SlowPlayer());

            Destroy(transform.parent.gameObject);
        }
        else if ((Ground & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private IEnumerator SlowPlayer()
    {
        PlayerStatManager.instance.statUp(
            StatType.SPEED,
            -15
        );

        yield return new WaitForSeconds(3f);

        PlayerStatManager.instance.statUp(
            StatType.SPEED,
            15
        );
    }
}