using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour, IHealth
{
    Animator ani;

    public GameObject parentCanvas;
    public GameObject _hitText;

    [SerializeField, Space(10f)]
    private float WaitHitTime;

    public float MaxHP { get; set; }
    public float HP { get; set; }
    public GameObject hitText { get; set; }

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void Die()
    {
        StatisticManager.Instance.OnStatistic();
    }

    public void Heal(float amount, GameObject healObject) { }

    public void Hurt(float damage, GameObject attacker)
    {
        if (ani.GetBool("isDie"))
        {
            return;
        }
        HitContext hit = new HitContext(damage, attacker);
        if (damage > 0)
        {
            AbilityManager.instance.NotifyHit(ref hit);
            WorkerHub<SoundWorker>.Instance.PlaySFX(
                GameManager.Instance.Source,
                GameManager.Instance.SFX.GetClip(SoundType.Hit)
            );
            StatisticManager.Instance.GetHurt((int)damage);
        }
        ShowHealthText(hit.FinalDamage, Color.red);
        StartCoroutine(WaitHitEffect());
        PlayerStatManager.instance.ChangeHealth(-hit.FinalDamage);
        if (PlayerStatManager.instance.currentHP <= 0)
        {
            ani.SetBool("isDie", true);
            ani.Play("Die");
            GameOver();
        }
    }

    private void GameOver()
    {
        Destroy(GetComponent<PlayerController>());
        Destroy(GetComponent<PlayerAttackSystem>());
        Destroy(GetComponent<PlayerInput>());
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
