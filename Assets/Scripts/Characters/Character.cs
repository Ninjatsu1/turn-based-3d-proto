using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    public CharacterStats CharacterStats;
    public static event Action<Character> CharacterEliminated;
    public static event Action<Character> CharacterCurrentHealth;
    public List<Skill> CharacterSkills = new List<Skill>();
    public string Name;
    public int Attack;
    public int MaxHealth;
    public int CurrentHealh;
    public int Speed;
    public bool IsPlayerCharacter;

    private void Awake()
    {
        Name = CharacterStats.Name;
        Attack = CharacterStats.Attack;
        MaxHealth = CharacterStats.MaximumHealth;
        Speed = CharacterStats.Speed;
        CurrentHealh = MaxHealth;
        CharacterSkills = CharacterStats.Skills;
        IsPlayerCharacter = CharacterStats.IsPlayerCharacter;
    }

    public void Damage(int damage)
    {
        Debug.Log(Name + " took: " + damage + "amount!");
        CurrentHealh = CurrentHealh - damage;
        CharacterCurrentHealth?.Invoke(this);
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
