using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputSystem_Actions inputActions { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
