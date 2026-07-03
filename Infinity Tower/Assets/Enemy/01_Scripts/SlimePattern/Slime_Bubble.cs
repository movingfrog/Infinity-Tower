using UnityEngine;

public partial class Slime
{
    partial void BubbleAttack()
    {
        Instantiate(
            AttackPattern[0],
            PlayerColl.transform.position + Vector3.down * PlayerColl.bounds.extents.y,
            Quaternion.identity
        );
    }
}
