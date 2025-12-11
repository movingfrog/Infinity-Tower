using UnityEngine;
using UnityEngine.InputSystem;

public class TimeBackSkill : MonoBehaviour
{
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

    void OnSkillAnimation(InputAction.CallbackContext callback) => ani.SetBool("isUsingSkill", true);
    void EndSkillAnimation(InputAction.CallbackContext callback) => ani.SetBool("isUsingSkill", false);
}
