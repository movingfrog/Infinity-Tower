using UnityEngine;

public abstract class SO_Goods : ScriptableObject
{
    [SerializeField]
    protected uint value;

    protected abstract string GoodsName { get; }

    public virtual uint Get() => value;

    public virtual void Increase(uint amount) => value = amount;

    public virtual bool Decrease(uint amount)
    {
        if (amount > value)
            return false;
        value -= amount;
        return true;
    }
}
