using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_FastFoot",
    menuName = "Scriptable Objects/TechData/SO_Ability/SO_FastFoot"
)]
public class SO_FastFootAbilityTech : SO_AbilityTechData, IOnAcquire
{
    public override string GetEffect(int level)
    {
        if (level <= 0)
            Debug.LogError("값이 정상적이지 않습니다");

        return string.Format(_effect, level - 1, level);
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, level);
    }

    public void OnAcquire(int level)
    {
        PlayerStatManager.instance.DashCount = level;
    }
}
