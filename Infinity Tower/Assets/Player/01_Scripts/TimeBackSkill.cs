using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class TimeBackSkill : MonoBehaviour
{
    bool checkRewind;
    Animator ani;
    InputSystem_Actions inputs;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        inputs = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputs.Player.Enable();

        inputs.Player.Skill.started += OnSkillAnimation;
        inputs.Player.Skill.canceled += EndSkillAnimation;
    }

    private void OnDisable()
    {
        inputs.Player.Skill.started -= OnSkillAnimation;
        inputs.Player.Skill.canceled -= EndSkillAnimation;

        inputs.Player.Disable();
    }
    private void FixedUpdate()
    {
        if(!TimeManager.Instance.isRewinding && checkRewind)
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
    void EndSkillAnimation(InputAction.CallbackContext callback) => ani.SetBool("isUsingSkill", false);
}
