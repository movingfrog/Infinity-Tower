using System.Collections;
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
    public TimeBody playerBody { get; private set; }

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
    private void Start()
    {
        StartCoroutine(setupPlayerBody());
    }

    IEnumerator setupPlayerBody()
    {
        yield return null;

        playerBody = timeBodies.Find(t => t.CompareTag("Player"));
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
        if (playerBody == null) return;
        if (_currentFrameAgo >= playerBody.currentCount/*플레이어 현재 기록의 수로 비교 필요*/)
        {
            Debug.Log("log");
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

        foreach(var obj in timeBodies)
        {
            obj.ResetTimeDataAfterRewind(_currentFrameAgo);
        }

        _currentFrameAgo = 0;
    }
}
