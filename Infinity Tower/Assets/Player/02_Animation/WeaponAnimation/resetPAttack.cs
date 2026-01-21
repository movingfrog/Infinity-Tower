using UnityEngine;

public class resetPAttack : StateMachineBehaviour
{
    [SerializeField] string attackTrigger;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(attackTrigger);
    }
}
