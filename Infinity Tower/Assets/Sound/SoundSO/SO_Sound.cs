using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Walk,
    Dash,
    Jump,
    GunAttack,
    p_Sword,
    p_Spear,
    BowCharge,
    BowAttack,
    Attack,
    Critical,
    Hit,
    Portal,
    Pickup,
    OpenChest,
    Purchase,
    MainBGM,
}

[CreateAssetMenu(fileName = "SO_Sound", menuName = "Scriptable Objects/Sound/SO_Sound")]
public class SO_Sound : ScriptableObject
{
    [SerializeField]
    private SO_SoundCilp[] allClip;

    public Dictionary<SoundType, AudioClip> getClipDic = new();

    public AudioClip GetClip(SoundType type) =>
        getClipDic.ContainsKey(type) ? getClipDic[type] : null;

    private void OnEnable()
    {
        SyncDictonary();
    }

    private void SyncDictonary()
    {
        getClipDic.Clear();

        foreach (var c in allClip)
        {
            if (!getClipDic.ContainsKey(c.type))
            {
                getClipDic.Add(c.type, c.clip);
            }
            else
            {
                Debug.LogError($"중복된 값이 들어왔습니다{c.type}");
            }
        }
    }
}
