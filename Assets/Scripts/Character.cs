using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterStats CharacterStats;

    public string Name;
    public int Attack;
    public int Health;
    public int Speed;

    private void Awake()
    {
         Name = CharacterStats.Name;
         Attack = CharacterStats.Attack;
         Health = CharacterStats.Health;
         Speed = CharacterStats.Speed;
    }

    public void DamageCharacter(int damageAmount)
    {
        Health = Health - damageAmount;
    }

    public void HealCharacter(int healAmount)
    {

    }
}
