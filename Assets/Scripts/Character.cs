using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    public CharacterStats CharacterStats;
    public static event Action<Character> CharacterEliminated;
    public string Name;
    public int Attack;
    public int Health;
    public int CurrentHealh;
    public int Speed;

    private void Awake()
    {
        Name = CharacterStats.Name;
        Attack = CharacterStats.Attack;
        Health = CharacterStats.MaximumHealth;
        Speed = CharacterStats.Speed;
        CurrentHealh = Health;
    }

    public void Damage(int damage)
    {
        CurrentHealh = CurrentHealh - damage;
        if (CurrentHealh <= 0)
        {
            Eliminate();
        }
    }

    public void Heal(int heal)
    {
        throw new System.NotImplementedException();
    }

    public void Eliminate()
    {
        Debug.Log("Eliminated: " + Name);
        CharacterEliminated?.Invoke(this);
        gameObject.SetActive(false);
    }


}
