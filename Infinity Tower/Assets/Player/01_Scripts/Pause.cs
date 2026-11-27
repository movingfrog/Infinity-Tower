using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseUI;

    [SerializeField]
    private GameObject[] StartOffUI;

    bool isPaused = false;

    private void Awake()
    {
        InputManager.Instance.inputActions.Player.Pause.started += PauseGame;
    }

    private void OnDestroy()
    {
        InputManager.Instance.inputActions.Player.Pause.started -= PauseGame;
    }

    public void PauseGame(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        PauseUI.SetActive(isPaused);
        foreach (var ui in StartOffUI)
        {
            ui.SetActive(false);
        }
    }

    public void PauseGameButton() => PauseGame(new InputAction.CallbackContext());

    public void Giveup()
    {
        PauseGame(new InputAction.CallbackContext());
        FindAnyObjectByType<PlayerHealth>().Die();
    }
}
