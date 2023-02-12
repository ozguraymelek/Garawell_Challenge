using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(Rigidbody rb);
}

public interface IPlayer
{
    void Die();
}

public interface IAI
{
    void Die();
}
