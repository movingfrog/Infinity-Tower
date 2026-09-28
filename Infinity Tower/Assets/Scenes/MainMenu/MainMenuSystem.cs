using UnityEngine;

public class MainMenuSystem : MonoBehaviour
{
    public void PlayGame()
    {
        SceneChangeManager.Instance.SceneChange(1);
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
