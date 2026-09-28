using UnityEngine;

public class StageMovePortal : Portal
{
    [SerializeField]
    private string BossStage;

    [SerializeField]
    protected string NormalStageName;

    [Space(10f), SerializeField]
    private int ClearAmount;
    private static int currentStageCount;

    protected override void TpPlayer(Collider2D player)
    {
        WorkerHub<SoundWorker>.Instance.PlaySFX(
            GameManager.Instance.Source,
            GameManager.Instance.SFX.GetClip(SoundType.Portal)
        );
        currentStageCount++;
        if (currentStageCount >= ClearAmount)
        {
            SceneChangeManager.Instance.SceneChange(BossStage);
            currentStageCount = 0;
        }
        else
        {
            SceneChangeManager.Instance.SceneChange(NormalStageName);
        }
    }
}
