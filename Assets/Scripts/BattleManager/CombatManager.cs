using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField]
    private List<CharacterStats> turnOrder = new List<CharacterStats>();
    [SerializeField]
    private CharacterStats currentCharactersTurn;

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
            turnOrder.Add(charactersArray[i].GetComponent<Character>().CharacterStats);
        }
    }

    private void SetTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(character => character.Speed).ToList();
    }

    private void CombatInitiation()
    {
        combatState = CombatState.Setup;
        currentCharactersTurn = turnOrder[0];
        CombatStarts?.Invoke();
        Combat();
    }

    private void Combat()
    {
        if(combatState != CombatState.Win || combatState != CombatState.Lost)
        {
            for (int i = 0; i < turnOrder.Count; i++)
            {
                if (turnOrder[i].IsPlayerCharacter)
                {
                    Debug.Log("Player turn");
                }
                else
                {
                    Debug.Log("Enemy turn");
                }
            }
        }
    }



    private IEnumerator PlayerTurn()
    {
        combatState = CombatState.PlayerTurn;
       Debug.Log("Player turn");
        yield return new WaitForSeconds(1.0f);
    }

    private void EnemyTurn()
    {
        combatState = CombatState.EnemyTurn;
        Debug.Log("Enemy turn");
    }
}
