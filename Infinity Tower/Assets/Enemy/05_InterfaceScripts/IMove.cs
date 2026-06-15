using UnityEngine;

public interface IMove
{
    public float Speed { get; set; }
    public abstract void Move();
}
