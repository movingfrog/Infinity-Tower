using UnityEngine;

public partial class Slime
{
    [SerializeField]
    private float s_Speed;

    partial void PoisionBubble()
    {
        GameObject Bubble = Instantiate(AttackPattern[1], transform.position, Quaternion.identity);
        Rigidbody2D Brigid = Bubble.GetComponent<Rigidbody2D>();
        Vector3 angle = PlayerColl.transform.position - transform.position;
        Brigid.AddForce(angle.normalized * s_Speed, ForceMode2D.Impulse);
    }
}
