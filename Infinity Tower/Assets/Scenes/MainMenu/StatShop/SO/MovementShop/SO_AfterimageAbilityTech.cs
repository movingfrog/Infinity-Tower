using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_AfterimageAbilityTech",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_AfterimageAbility"
)]
public class SO_AfterimageAbilityTech : SO_AbilityTechData, IOnHit
{
    [SerializeField]
    private float d_Percent;

    public override string GetEffect(int level)
    {
        if (level <= 0)
            Debug.LogError("값이 정상적이지 않습니다");

        return string.Format(_effect, (level - 1) * d_Percent, level * d_Percent);
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, level * d_Percent);
    }

    public void OnHit(int level, ref HitContext ctx)
    {
        if (UnityEngine.Random.Range(0, 100) < d_Percent * level)
        {
            ctx.FinalDamage = 0;
            Debug.Log($"성공: 변환된 데미지{ctx.FinalDamage}");
        }
    }
}
