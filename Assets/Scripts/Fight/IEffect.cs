using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public double Value { get; }
    void ActivateEffect(GameObject gameObject);
}

abstract class Effect : IEffect
{
    public double Value { get; }

    public Effect(double value)
    {
        this.Value = value;
    }
    public abstract void ActivateEffect(GameObject gameObject);
}

class FireEffect : Effect
{
    public FireEffect(double value) : base(value) { }

    public override void ActivateEffect(GameObject gameObject)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(Value);
    }
}

class SlovEffect : Effect
{
    public double Time { get; }
    public SlovEffect(double value, double time) : base(value) 
    {
        this.Time = time;
    }
    public override void ActivateEffect(GameObject gameObject)
    {
        IMoveable moveable;
        if (gameObject.TryGetComponent<IMoveable>(out moveable)) moveable.GetSlowEffect(Value, Time);
    }
}

class NullEffect : IEffect
{
    public double Value { get; }

    public NullEffect() { Value = 0; }
    public void ActivateEffect(GameObject gameObject) { }
}
}