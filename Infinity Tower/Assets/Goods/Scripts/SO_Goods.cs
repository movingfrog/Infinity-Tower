using UnityEngine;

public abstract class SO_Goods : ScriptableObject
{
    [SerializeField]
    protected uint value;

    [SerializeField]
    protected GoodsType type;

    protected abstract string GoodsName { get; }

    public virtual uint Get => value;

    public virtual GoodsType Type => type;

    public virtual void Increase(uint amount) => value += amount;

    public virtual bool Decrease(uint amount)
    {
        if (amount > value)
            return false;
        value -= amount;
        return true;
    }
}
