using UnityEngine;

[CreateAssetMenu(fileName = "SO_SoundCilp", menuName = "Scriptable Objects/Sound/SO_SoundCilp")]
public class SO_SoundCilp : ScriptableObject
{
    public SoundType type;
    public AudioClip clip;
}
