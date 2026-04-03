using UnityEngine;
using UnityEngine.InputSystem;

public class TimeBackSkill : MonoBehaviour
{
    bool checkRewind;
    Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Skill.started += OnSkillAnimation;
        InputManager.Instance.inputActions.Player.Skill.canceled += EndSkillAnimation;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Skill.started -= OnSkillAnimation;
        InputManager.Instance.inputActions.Player.Skill.canceled -= EndSkillAnimation;
    }

    private void FixedUpdate()
    {
        if (!TimeManager.Instance.isRewinding && checkRewind)
        {
            ani.SetBool("isUsingSkill", false);
            checkRewind = false;
        }
    }

    void OnSkillAnimation(InputAction.CallbackContext callback)
    {
        ani.SetBool("isUsingSkill", true);
        ani.SetTrigger("useSkill");
        ani.SetFloat("Velo", 0);
        checkRewind = true;
    }

    void EndSkillAnimation(InputAction.CallbackContext callback) =>
        ani.SetBool("isUsingSkill", false);
}
