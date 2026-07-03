using UnityEngine;

public class ReturnKingSlime : MonoBehaviour
{
    private static Transform PlayerPos;

    [SerializeField]
    private KingSlime kingSlime;

    private void Start()
    {
        if (PlayerPos == null)
            PlayerPos = FindAnyObjectByType<PlayerController>().transform;
    }

    private void OnDestroy()
    {
        kingSlime.PhaseChange(PlayerPos);
    }
}
