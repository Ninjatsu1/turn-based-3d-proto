using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private List<Character> turnOrder = new List<Character>();
    [SerializeField]
    private Character currentCharactersTurn;
    [SerializeField]
    private bool playerDidAction = false;
    [SerializeField]
    private bool battleEnded = false;
    [SerializeField]
    private bool enemyDidAction = false;

    public static event Action<CombatState> CurrentCombatPhase;
    public static event Action<Character> EnemyAction;
    public CombatState combatState;

    private void OnEnable()
    {
        PlayerCombatActions.PlayerDidAction += PlayerDidAction;
        EnemyCombatActions.EnemyDidAction += EnemyDidAction;
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
            turnOrder.Add(charactersArray[i].GetComponent<Character>());
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
                    if (turnOrder[i].CharacterStats.IsPlayerCharacter)
                    {
                        enemyDidAction = false;
                        StartCoroutine(PlayerTurn());
                        yield return new WaitUntil(() => playerDidAction == true);
                    }
                    else
                    {
                        playerDidAction = false;
                        StartCoroutine(EnemyTurn(currentCharactersTurn));
                        yield return new WaitUntil(() => enemyDidAction == true);

                    }
                }
            }
        }
        Debug.Log("Battle ended");
        StopCoroutine(PlayerTurn());
        StopCoroutine(EnemyTurn(null));
    }

    private void RemoveCharacterFromTurnOrder(Character characterToRemove)
    {
        if(characterToRemove.IsPlayerCharacter)
        {
            CheckPlayersAtRemoval(characterToRemove);
        }
        turnOrder.Remove(characterToRemove);
        if(turnOrder.Count == 1)
        {
            PlayerWin();
        }
    }

    private void PlayerWin()
    {
        battleEnded = true;
        combatState = CombatState.Win;
    }

    private void CheckPlayersAtRemoval(Character characterToRemove)
    {
        battleEnded = true;
        combatState = CombatState.Lost;
    }

    private IEnumerator PlayerTurn()
    {
        StopCoroutine(EnemyTurn(null));
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

    private IEnumerator EnemyTurn(Character currentEnemy)
    {
        StopCoroutine(PlayerTurn());
        combatState = CombatState.EnemyTurn;
        EnemyAction?.Invoke(currentCharactersTurn);
        CurrentCombatPhase?.Invoke(CombatState.EnemyTurn);
        var enemy = currentEnemy.GetComponent<Character>();
        Debug.Log("HEALTH: " + enemy.CurrentHealh);
        while (!enemyDidAction)
        {
            yield return null;
        }
    }

    private void EnemyDidAction(bool enemyFinishedAction)
    {
        enemyDidAction = enemyFinishedAction;
    }

    private void OnDisable()
    {
        PlayerCombatActions.PlayerDidAction -= PlayerDidAction;
        EnemyCombatActions.EnemyDidAction -= EnemyDidAction;
        Character.CharacterEliminated -= RemoveCharacterFromTurnOrder;
    }
}