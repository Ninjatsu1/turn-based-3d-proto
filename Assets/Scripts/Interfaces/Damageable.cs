using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(int damage);
    void Heal(int heal);
    void Eliminate();
}
