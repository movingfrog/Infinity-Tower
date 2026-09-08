using UnityEngine;

public partial class Frog
{
    [SerializeField]
    private float s_Speed;

    partial void TongueAttack()
    {
        Vector3 target = PlayerColl.transform.position;

        StartCoroutine(
            waitAttackCool(
                .4f,
                () =>
                {
                    GameObject Tongue =
                        Instantiate(
                            AttackPattern[0],
                            transform.position,
                            Quaternion.identity
                        );

                    Rigidbody2D Trigid =
                        Tongue.GetComponent<Rigidbody2D>();

                    Vector3 angle =
                        target - transform.position;

                    Trigid.AddForce(
                        angle.normalized * s_Speed,
                        ForceMode2D.Impulse
                    );
                }
            )
        );
    }
}