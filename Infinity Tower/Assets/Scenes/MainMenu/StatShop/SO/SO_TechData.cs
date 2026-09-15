using UnityEngine;

public class SO_TechData : ScriptableObject
{
    [field: SerializeField]
    public Sprite TechIcon;

    [field: SerializeField]
    public string Name { get; private set; }

    /// <summary>
    /// Expane의 숫자 값은 0과 1로 setTexting하기
    /// </summary>
    [SerializeField, TextArea, Tooltip("0과 1을 활용하여 값 할당")]
    protected string _explane;

    public virtual string GetExplane(int level) => _explane;

    /// <summary>
    /// Effect의 숫자 값은 0과 1로 setTexting하기
    /// </summary>
    [field: SerializeField, TextArea, Tooltip("0과 1을 활용하여 값 할당")]
    protected string _effect;

    public virtual string GetEffect(int level) => _effect;

    [field: SerializeField]
    public uint UseAmount { get; private set; }

    public virtual void Apply(int level) { }
}
