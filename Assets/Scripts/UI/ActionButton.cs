using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private GameObject currentTarget;

    public static event Action PlayerAttack;
    public CombatState combatState;


    private void Awake()
    {
        attackButton.onClick.AddListener(OnActionButton);
    }

    private void OnEnable()
    {
        CombatManager.CurrentCombatPhase += DisableButton;
        PlayerCombatActions.SetPlayerTarget += SetTarget;
        PlayerCombatActions.RemovePlayerTarget += RemoveTarget;
    }

    private void Update()
    {
        if(currentTarget == null)
        {
            attackButton.interactable = false;
        } else
        {
            attackButton.interactable = true;
        }
    }

    private void OnActionButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke();
    }

    private void SetTarget(GameObject target)
    {
        currentTarget = target;
    }

    private void RemoveTarget()
    {
        currentTarget = null;
    }

    private void DisableButton(CombatState combatState)
    {
        switch (combatState)
        {
            case CombatState.Setup:
                break;
            case CombatState.PlayerTurn:
                attackButton.interactable = true;
                break;
            case CombatState.EnemyTurn:
                attackButton.interactable = false;
                break;
            case CombatState.Win:
                break;
            case CombatState.Lost:
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        attackButton.onClick.RemoveListener(OnActionButton);
        CombatManager.CurrentCombatPhase -= DisableButton;
        PlayerCombatActions.SetPlayerTarget -= SetTarget;
        PlayerCombatActions.RemovePlayerTarget -= RemoveTarget;
    }
}
