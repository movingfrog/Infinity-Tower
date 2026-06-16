using System.Collections;
using UnityEngine;

public class PlayBGAnimation : MonoBehaviour
{
    Animator ani;

    [SerializeField]
    private float runTime;

    private void Awake()
    {
        if (TryGetComponent(out ani))
            StartCoroutine(AnimationPlay());
    }

    IEnumerator AnimationPlay()
    {
        if (runTime <= 0)
            runTime = 1;

        while (true)
        {
            ani.SetBool("Play", true);
            yield return new WaitForSeconds(runTime / 2);
            ani.SetBool("Play", false);
            yield return new WaitForSeconds(runTime / 2);
        }
    }
}
