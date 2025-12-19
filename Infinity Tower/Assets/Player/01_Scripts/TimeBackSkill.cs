using Unity.VisualScripting;
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
        inputs.Player.Skill.performed += CheckRecord;
        inputs.Player.Skill.canceled += EndSkillAnimation;
    }

    private void OnDisable()
    {
        inputs.Player.Skill.started -= OnSkillAnimation;
        inputs.Player.Skill.performed -= CheckRecord;
        inputs.Player.Skill.canceled -= EndSkillAnimation;

        inputs.Player.Disable();
    }

    void OnSkillAnimation(InputAction.CallbackContext callback)
    {
        ani.SetBool("isUsingSkill", true);
        ani.SetTrigger("useSkill");
    }
    void CheckRecord(InputAction.CallbackContext callback)
    {
        if (TimeManager.Instance._currentFrameAgo >= TimeBody.MAX_recordTime) ani.SetBool("isUsingSkill", false);
    }
    void EndSkillAnimation(InputAction.CallbackContext callback) => ani.SetBool("isUsingSkill", false);
}
