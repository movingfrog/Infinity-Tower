using UnityEngine;

public class NextStageMovePortal : StageMovePortal
{
    public static int currentStage { get; private set; }

    [SerializeField]
    private bool isLastBoss;

    protected override void TpPlayer(Collider2D player)
    {
        WorkerHub<SoundWorker>.Instance.PlaySFX(
            GameManager.Instance.Source,
            GameManager.Instance.SFX.GetClip(SoundType.Portal)
        );
        currentStage++;
        if (isLastBoss)
            SceneChangeManager.Instance.SceneChange(BossStage);
        else
            SceneChangeManager.Instance.SceneChange(NormalStageName);
    }
}
