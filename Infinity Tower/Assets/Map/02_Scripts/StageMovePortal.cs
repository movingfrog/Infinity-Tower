using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMovePortal : Portal
{
    [SerializeField]
    private string StageName;

    protected override void TpPlayer(Collider2D player)
    {
        SceneManager.LoadScene(StageName);
    }
}
