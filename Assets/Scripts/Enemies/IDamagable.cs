using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamagable
{
    public bool IsAlive { get;}
    public float Health { get; }
    public float StartHealth { get; }
    public float AttackValue { get; }

    public UnityEvent OnDie { get; }
    public UnityEvent OnCreate { get; }
    public UnityEvent OnHealthChanged { get; }

    public virtual void GetDamage(float damageVal) { }
}
