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

    private void Start()
    {
        attackButton.interactable = false;
    }

    private void OnActionButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke();
        attackButton.interactable = false;
    }

    private void SetTarget(Character target)
    {
        attackButton.interactable = true;
        currentTarget = target.gameObject;
    }

    private void RemoveTarget()
    {
        attackButton.interactable = false;
        currentTarget = null;
    }

    private void DisableButton(CombatState combatState)
    {
        Debug.Log("Button state: " + combatState);
        switch (combatState)
        {
            case CombatState.Setup:
                break;
            case CombatState.PlayerTurn:
                if(currentTarget != null)
                attackButton.interactable = true;
                break;
            case CombatState.EnemyTurn:
                attackButton.interactable = false;
                break;
            case CombatState.Win:
                attackButton.interactable = false;
                break;
            case CombatState.Lost:
                attackButton.interactable = false;
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
