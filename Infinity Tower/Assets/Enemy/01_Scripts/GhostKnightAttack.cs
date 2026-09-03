using UnityEngine;
using UnityEngine.Events;

public class GhostKnightAttack : MonoBehaviour
{
    public UnityEvent onAttackEvent { get; private set; } = new UnityEvent();

    public void OnAttack()
    {
        onAttackEvent?.Invoke();
    }
}
