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
    [SerializeField]
    private bool playerDidAction = false;
    [SerializeField]
    private bool battleEnded = false;
    [SerializeField]
    private bool enemyDidAction = false;

    public static event Action<CombatState> CurrentCombatPhase;
    public CombatState combatState;

    private void OnEnable()
    {
        PlayerCombatActions.PlayerDidAction += PlayerDidAction;
        Character.CharacterEliminated += RemoveCharacterFromTurnOrder;
    }


    private void Start()
    {
        GetCharacters();
        SetTurnOrder();
        CombatInitiation();
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

        CurrentCombatPhase?.Invoke(CombatState.Setup);
        StartCoroutine(Combat());
    }

    private IEnumerator Combat()
    {
        while (!battleEnded)
        {
            if (battleEnded)
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < turnOrder.Count; i++)
                {
                    currentCharactersTurn = turnOrder[i];
                    if (turnOrder[i].IsPlayerCharacter)
                    {
                        enemyDidAction = false;
                        Debug.Log("Current turn: " + currentCharactersTurn);
                        StartCoroutine(PlayerTurn());
                        yield return new WaitUntil(() => playerDidAction == true);
                    }
                    else
                    {
                        Debug.Log("Current turn: " + currentCharactersTurn);
                        playerDidAction = false;
                        StartCoroutine(EnemyTurn());
                        yield return new WaitUntil(() => enemyDidAction == true);

                    }
                }
            }
        }
        Debug.Log("Battle ended");
        StopCoroutine(PlayerTurn());
        StopCoroutine(EnemyTurn());
    }

    private void RemoveCharacterFromTurnOrder(Character characterToRemove)
    {
        turnOrder.Remove(characterToRemove.CharacterStats);
    }

    private IEnumerator PlayerTurn()
    {
        StopCoroutine(EnemyTurn());
        combatState = CombatState.PlayerTurn;
        Debug.Log("Player turn");
        CurrentCombatPhase?.Invoke(CombatState.PlayerTurn);
        while (!playerDidAction)
        {
            yield return null;
        }
    }

    private void PlayerDidAction(bool actionDone)
    {
        playerDidAction = actionDone;
    }

    private IEnumerator EnemyTurn()
    {
        StopCoroutine(PlayerTurn());
        combatState = CombatState.EnemyTurn;
        CurrentCombatPhase?.Invoke(CombatState.EnemyTurn);
        
        while (!enemyDidAction)
        {
            yield return null;
        }
    }

    private void EnemyDidAction()
    {
        enemyDidAction = true;
    }

    private void OnDisable()
    {
        PlayerCombatActions.PlayerDidAction -= PlayerDidAction;
        Character.CharacterEliminated -= RemoveCharacterFromTurnOrder;
    }
}