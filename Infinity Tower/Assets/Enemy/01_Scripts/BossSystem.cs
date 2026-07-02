using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossSystem : parentEnemy
{
    protected List<System.Func<IEnumerator>> patternPool = new List<System.Func<IEnumerator>>();

    [SerializeField]
    protected float WaitNewAction = 1f;

    [SerializeField]
    protected float GroggyTime;

    protected override void Awake()
    {
        base.Awake();

        AddPattern();
    }

    protected virtual void Start()
    {
        StartCoroutine(BossActionLoop());
    }

    protected abstract void AddPattern();

    protected virtual IEnumerator BossActionLoop()
    {
        yield return null;

        while (!isDie)
        {
            ani.SetTrigger("isIdle");

            yield return new WaitForSeconds(WaitNewAction);

            if (TimeManager.Instance.isRewinding)
                continue;

            if (patternPool.Count <= 0)
            {
                Debug.LogError("패턴 없음");
                break;
            }

            int randIndex = Random.Range(0, patternPool.Count);
            System.Func<IEnumerator> selectedPattern = patternPool[randIndex];
            yield return StartCoroutine(selectedPattern());
        }
    }

    protected abstract IEnumerator Groggy();
}
