using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private List<CharacterStats> characters = new List<CharacterStats>();

    [SerializeField] 
    private List<CharacterStats> turnOrder = new List<CharacterStats>();

    public static event Action CombatStarts;
    public CombatState combatState;

    private void Start()
    {
        GetCharacters();
        SetTurnOrder();
        CombatInitiation();
        //Start combat
    }

    //Gets player and enemies
    private void GetCharacters() 
    {
        Character[] charactersArray;
        charactersArray = FindObjectsOfType<Character>();
        for (int i = 0; i < charactersArray.Length; i++)
        {
            characters.Add(charactersArray[i].GetComponent<Character>().CharacterStats);
        }
    }

    private void SetTurnOrder()
    {
       turnOrder = characters.OrderByDescending(character => character.Speed).ToList();
    }

    private void CombatInitiation()
    {
        combatState = CombatState.Setup;
        CombatStarts?.Invoke();

    }

    private void Combat()
    {
            
    }

    

    private void PlayerTurn() 
    {
        Debug.Log("Player turn");
    }

    private void EnemyTurn()
    {
        Debug.Log("Enemy turn");
    }
}
