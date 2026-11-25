using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseUI;

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
    }
}
