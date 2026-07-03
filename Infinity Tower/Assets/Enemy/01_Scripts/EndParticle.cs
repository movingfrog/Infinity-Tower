using UnityEngine;

public class EndParticle : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (particle != null && !particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
