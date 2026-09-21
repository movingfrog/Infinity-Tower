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
    public static int currentStage { get; private set; }

    protected override void TpPlayer(Collider2D player)
    {
        WorkerHub<SoundWorker>.Instance.PlaySFX(
            GameManager.Instance.Source,
            GameManager.Instance.SFX.GetClip(SoundType.Portal)
        );
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
