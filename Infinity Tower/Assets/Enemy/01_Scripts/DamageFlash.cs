using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField]
    private Color _flashColor = Color.white;

    [SerializeField]
    private float _flashTime = 1.0f;

    [SerializeField]
    private AnimationCurve _flashSpeedCurve;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        _material = _spriteRenderer.material;
    }

    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        _material.SetColor("_FlashColor", _flashColor);

        //lerp로 float값 변환
        float currentFlashAmount;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(
                1f,
                _flashSpeedCurve.Evaluate(elapsedTime),
                (elapsedTime / _flashTime)
            );
            _material.SetFloat("_FlashAmount", currentFlashAmount);

            yield return null;
        }
    }
}
