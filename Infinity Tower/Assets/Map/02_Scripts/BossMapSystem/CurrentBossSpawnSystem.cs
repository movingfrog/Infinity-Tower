using UnityEngine;

public class CurrentBossSpawnSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] BossMap;

    private void Awake()
    {
        Instantiate(BossMap[NextStageMovePortal.currentStage], transform);
    }
}
