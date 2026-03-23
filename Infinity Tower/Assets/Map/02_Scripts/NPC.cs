using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject interactionIcon;
    public LayerMask PlayerLayer;
    public float radius;
    public bool isIn;

    protected void FixedUpdate()
    {
        Collider2D Player = Physics2D.OverlapCircle(transform.position, radius, PlayerLayer);

        interactionIcon.SetActive(Player != null);
        isIn = Player != null;
        if (Player != null)
        {
            interactionIcon.transform.position = Player.transform.position;
        }
    }

    public void OnInteract()
    {
        if (isIn) { }
    }
}
