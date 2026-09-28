using UnityEngine;

public class NextStageMovePortal : StageMovePortal
{
    public static int currentStage { get; private set; }

    protected override void TpPlayer(Collider2D player)
    {
        WorkerHub<SoundWorker>.Instance.PlaySFX(
            GameManager.Instance.Source,
            GameManager.Instance.SFX.GetClip(SoundType.Portal)
        );
        currentStage++;
        SceneChangeManager.Instance.SceneChange(NormalStageName);
    }
}
