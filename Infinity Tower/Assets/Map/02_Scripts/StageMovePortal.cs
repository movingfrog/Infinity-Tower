using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMovePortal : Portal
{
    [SerializeField]
    private string BossStage;

    [SerializeField]
    private string NormalStageName;

    [Space(10f), SerializeField]
    private int ClearAmount;
    private static int currentStage;

    protected override void TpPlayer(Collider2D player)
    {
        currentStage++;
        if (currentStage >= ClearAmount)
        {
            SceneManager.LoadScene(BossStage);
            currentStage -= ClearAmount;
        }
        else
        {
            SceneManager.LoadScene(NormalStageName);
            currentStage++;
        }
    }
}
