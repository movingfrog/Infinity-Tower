using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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
