using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public GameObject parentCanvas;
    public GameObject _hitText;

    [SerializeField, Space(10f)]
    private float WaitHitTime;

    public float MaxHP { get; set; }
    public float HP { get; set; }
    public GameObject hitText { get; set; }

    public void Die() { }

    public void Heal(float amount, GameObject healObject) { }

    public void Hurt(float damage)
    {
        ShowHealthText(damage, Color.red);
        StartCoroutine(WaitHitEffect());
        PlayerStatManager.instance.ChangeHealth(-damage);
    }

    private void ShowHealthText(float value, Color color)
    {
        if (value == 0)
            return;
        GameObject hitTextInstance = Instantiate(_hitText, parentCanvas.transform);
        Rigidbody2D rigid = hitTextInstance.GetComponent<Rigidbody2D>();
        TextMeshProUGUI text = hitTextInstance.GetComponent<TextMeshProUGUI>();

        float randAmount = Random.Range(.5f, -.5f);

        text.color = color;
        text.text = value.ToString("0");
        hitTextInstance.transform.position = transform.position;
        rigid.AddForce(new Vector2(randAmount, 5), ForceMode2D.Impulse);

        Destroy(hitTextInstance, 1f);
    }

    private IEnumerator WaitHitEffect()
    {
        float t = 0;
        while (t < WaitHitTime)
        {
            t += Time.deltaTime;
            gameObject.layer = 8;
            yield return null;
        }
        gameObject.layer = 7;
    }
}
