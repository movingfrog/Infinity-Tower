using System.Collections;
using UnityEngine;

public class PlayBGAnimation : MonoBehaviour
{
    Animator ani;

    [SerializeField]
    private float runTime;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(AnimationPlay());
    }

    IEnumerator AnimationPlay()
    {
        while (true)
        {
            ani.SetBool("Play", true);
            yield return new WaitForSeconds(runTime / 2);
            ani.SetBool("Play", false);
            yield return new WaitForSeconds(runTime / 2);
        }
    }
}
