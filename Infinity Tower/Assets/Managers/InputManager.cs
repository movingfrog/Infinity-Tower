using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    private static InputSystem_Actions _inputAction;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<InputManager>();
                if (_instance == null)
                {
                    var obj = new GameObject("InputManager");
                    _instance = obj.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
    }

    public InputSystem_Actions inputActions
    {
        get
        {
            if (_inputAction == null)
            {
                _inputAction = new InputSystem_Actions();
                _inputAction.Enable();
            }
            return _inputAction;
        }
    }

    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_instance != null)
            _instance = null;
        if (_inputAction != null)
            _inputAction = null;
    }
}
