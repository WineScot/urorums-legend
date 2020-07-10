using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION { LEFT, RIGHT, TOP, BOTTOM, AREA}

public interface IDamage
{
    public IEffect Effect { get; }
    public double DamageValue { get; }
    public void DealDamage(GameObject gameObject);
    public void DealDamage(GameObject gameObject, double damageValue);
}

abstract class Damage : IDamage
{
    protected DIRECTION direction;
    public double DamageValue { get; }
    public IEffect Effect { get; }
    public Damage(double damageValue, DIRECTION direction, IEffect effect)
    {
        this.DamageValue = damageValue;
        this.direction = direction;
        this.Effect = effect == null ? effect : new NullEffect();
    }

    public abstract void DealDamage(GameObject gameObject);

    public abstract void DealDamage(GameObject gameObject, double damageValue);
}

class TrueDamage : Damage
{
    public TrueDamage(double damageValue, DIRECTION direction, IEffect effect = null) : base(damageValue, direction, effect) { }

    public override void DealDamage(GameObject gameObject)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(DamageValue);
    }

    public override void DealDamage(GameObject gameObject, double damageValue)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(damageValue);
    }
}

class PhysicalDamage : Damage
{
    public PhysicalDamage(double damageValue, DIRECTION direction, IEffect effect = null) : base(damageValue, direction, effect) { }
    public override void DealDamage(GameObject gameObject)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(DamageValue);
    }

    public override void DealDamage(GameObject gameObject, double damageValue)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(damageValue);
    }
}

class MagicalDamage : Damage
{
    public MagicalDamage(double damageValue, DIRECTION direction, IEffect effect = null) : base(damageValue, direction, effect) { }
    public override void DealDamage(GameObject gameObject)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(DamageValue);
    }

    public override void DealDamage(GameObject gameObject, double damageValue)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character)) character.TakeDamage(damageValue);
    }
}

class CriticalDamage : Damage
{
    protected double probability;
    System.Random random = new System.Random();
    public CriticalDamage(double probability, double damageValue, DIRECTION direction, IEffect effect = null) : base(damageValue, direction, effect) 
    {
        this.probability = probability;
    }
    public override void DealDamage(GameObject gameObject)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character) && random.Next()%100+1 > (100- probability)) character.TakeDamage(DamageValue);
    }

    public override void DealDamage(GameObject gameObject, double damageValue)
    {
        ICharacter character;
        if (gameObject.TryGetComponent<ICharacter>(out character) && random.Next() % 100 + 1 > (100 - probability)) character.TakeDamage(damageValue);
    }
}

class PercentReduceDamage : IDamage
{
    private IDamage damage;
    double percent;
    public double DamageValue => damage.DamageValue * (100 - percent) / 100;

    public IEffect Effect => damage.Effect;

    public PercentReduceDamage(double percent, IDamage damage)
    {
        this.percent = Math.Max(0, Math.Min(percent, 100));
        this.damage = damage;
    }

    public void DealDamage(GameObject gameObject)
    {
        damage.DealDamage(gameObject, DamageValue);
    }

    public void DealDamage(GameObject gameObject, double damageValue)
    {
        damage.DealDamage(gameObject, damageValue);
    }
}

class NumericalReduceDamage : IDamage
{
    private IDamage damage;
    double value;
    public double DamageValue { get { return Math.Max(0, damage.DamageValue - value); } }

    public IEffect Effect => damage.Effect;

    public NumericalReduceDamage(double value, IDamage damage)
    {
        this.value = Math.Max(0,value);
        this.damage = damage;
    }

    public void DealDamage(GameObject gameObject)
    {
        damage.DealDamage(gameObject, DamageValue);
    }

    public void DealDamage(GameObject gameObject, double damageValue)
    {
        damage.DealDamage(gameObject, damageValue);
    }
}

class NewEffectDamage : IDamage
{
    private IDamage damage;
    public double DamageValue => damage.DamageValue;

    public IEffect Effect { get; }

    public NewEffectDamage(IEffect effect, IDamage damage)
    {
        this.Effect = effect;
        this.damage = damage;
    }

    public void DealDamage(GameObject gameObject)
    {
        damage.DealDamage(gameObject, DamageValue);
    }

    public void DealDamage(GameObject gameObject, double damageValue)
    {
        damage.DealDamage(gameObject, damageValue);
    }
}

class SpecialFunctionDamage