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

    public static event Action<CombatState> CurrentCombatPhase;
    public CombatState combatState;

    private void OnEnable()
    {
        PlayerCombatActions.PlayerDidAction += PlayerDidAction;
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
                    Debug.Log("Current characters turn: " + currentCharactersTurn);
                    if (turnOrder[i].IsPlayerCharacter)
                    {
                        StartCoroutine(PlayerTurn());
                        yield return new WaitUntil(() => playerDidAction == true);
                    }
                    else
                    {
                        playerDidAction = false;
                        StartCoroutine(EnemyTurn());
                    }
                }
            }
        }
        Debug.Log("Battle ended");
        StopCoroutine(PlayerTurn());
        StopCoroutine(EnemyTurn());
    }


    private IEnumerator PlayerTurn()
    {
        combatState = CombatState.PlayerTurn;
        Debug.Log("Player turn");
        while (!playerDidAction)
        {
            yield return null;
        }
    }

    private void PlayerDidAction(bool actionDone)
    {
        playerDidAction = actionDone;
        Debug.Log("Player did action: " + playerDidAction);

    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Player did action: " + playerDidAction);
        combatState = CombatState.EnemyTurn;
        Debug.Log("Enemy turn");
        yield return new WaitForSeconds(1f);
    }
}