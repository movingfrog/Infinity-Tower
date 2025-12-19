using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;
    public static TimeManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public List<TimeBody> timeBodies;

    public bool isRewinding { get; private set; }
    public int _currentFrameAgo = 0;
    private InputSystem_Actions inputs;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputs.Player.Enable();

        inputs.Player.Skill.started += StartRewind;
        inputs.Player.Skill.canceled += StopRewind;
    }
    private void OnDisable()
    {
        inputs.Player.Skill.started -= StartRewind;
        inputs.Player.Skill.canceled -= StopRewind;

        inputs.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (_currentFrameAgo >= TimeBody.MAX_recordTime)
        {
            StopRewind();
        }
        if (isRewinding)
        {
            _currentFrameAgo++;

            foreach(var obj in timeBodies)
            {
                obj.Rewind(_currentFrameAgo);
            }
        }
    }

    public void StartRewind(InputAction.CallbackContext callback)
    {
        isRewinding = true;
        _currentFrameAgo = 0;
    }
    
    public void StopRewind(InputAction.CallbackContext callback)
    {
        StopRewind();
    }
    public void StopRewind()
    {
        isRewinding = false;

    }
}
