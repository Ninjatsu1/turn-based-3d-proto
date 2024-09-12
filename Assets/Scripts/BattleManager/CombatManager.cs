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
        PlayerCombatActions.PlayerDidAction += OnPlayerDidAction;
        EnemyCombatActions.EnemyDidAction += OnEnemyDidAction;
        Character.CharacterEliminated += OnCharacterEliminated;
    }

    private void Start()
    {
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        PopulateTurnOrder();
        SortTurnOrder();
        StartCombat();
    }

    private void PopulateTurnOrder()
    {
        turnOrder = FindObjectsOfType<Character>().ToList();
    }

    private void SortTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(character => character.Speed).ToList();
    }

    private void StartCombat()
    {
        combatState = CombatState.Setup;
        currentCharactersTurn = turnOrder.FirstOrDefault();
        CurrentCombatPhase?.Invoke(combatState);
        StartCoroutine(CombatRoutine());
    }

    private IEnumerator CombatRoutine()
    {
        while (!battleEnded)
        {
            foreach (var character in turnOrder)
            {
                currentCharactersTurn = character;
                if (character.CharacterStats.IsPlayerCharacter)
                {
                    enemyDidAction = false;
                    yield return PlayerTurnRoutine();
                }
                else
                {
                    playerDidAction = false;
                    yield return EnemyTurnRoutine(character);
                }
            }
        }
        Debug.Log("Battle ended");
    }

    private IEnumerator PlayerTurnRoutine()
    {
        combatState = CombatState.PlayerTurn;
        CurrentCombatPhase?.Invoke(combatState);
        yield return new WaitUntil(() => playerDidAction);
    }

    private IEnumerator EnemyTurnRoutine(Character enemyCharacter)
    {
        combatState = CombatState.EnemyTurn;
        CurrentCombatPhase?.Invoke(combatState);
        EnemyAction?.Invoke(enemyCharacter);
        yield return new WaitUntil(() => enemyDidAction);
    }

    private void OnCharacterEliminated(Character eliminatedCharacter)
    {
        turnOrder.Remove(eliminatedCharacter);
        if (turnOrder.Count == 1)
        {
            PlayerWin();
        }
        else if (eliminatedCharacter.IsPlayerCharacter)
        {
            PlayerLose();
        }
    }

    private void PlayerWin()
    {
        battleEnded = true;
        combatState = CombatState.Win;
        CurrentCombatPhase?.Invoke(combatState);
    }

    private void PlayerLose()
    {
        battleEnded = true;
        combatState = CombatState.Lost;
        CurrentCombatPhase?.Invoke(combatState);
    }

    private void OnPlayerDidAction(bool actionDone)
    {
        playerDidAction = actionDone;
    }

    private void OnEnemyDidAction(bool actionDone)
    {
        enemyDidAction = actionDone;
    }

    private void OnDisable()
    {
        PlayerCombatActions.PlayerDidAction -= OnPlayerDidAction;
        EnemyCombatActions.EnemyDidAction -= OnEnemyDidAction;
        Character.CharacterEliminated -= OnCharacterEliminated;
    }
}
