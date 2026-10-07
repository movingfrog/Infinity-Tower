using UnityEngine;

public class MainMenuSystem : MonoBehaviour
{
    [SerializeField]
    private string SceneName;

    public void PlayGame()
    {
        SceneChangeManager.Instance.SceneChange(SceneName);
    }

    public void Setting()
    {
        Debug.Log("설정 코드 필요");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
