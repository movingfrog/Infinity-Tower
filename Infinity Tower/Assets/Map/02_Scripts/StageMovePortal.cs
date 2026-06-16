using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMovePortal : Portal
{
    [SerializeField]
    private string StageName;

    [SerializeField]
    private bool isProto;
    private static int currentStage;

    protected override void TpPlayer(Collider2D player)
    {
        currentStage++;
        if (isProto && currentStage >= 2)
            SceneManager.LoadScene(2);
        SceneManager.LoadScene(StageName);
    }
}
