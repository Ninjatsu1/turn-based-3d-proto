using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour
{
    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private GameObject currentTarget;
    [SerializeField]
    private Skill skill;
    [SerializeField]
    private TextMeshProUGUI buttonText;

    private Image buttonImage;
    private Color buttonColor;
    private event Action DisableButton;

    public static event Action<Skill> PlayerAttack;
    public static event Action RemoveTarget;


    private void Awake()
    {
        attackButton.onClick.AddListener(OnActionButton);
        buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        CombatManager.CurrentCombatPhase += BasedOnCombatState;
        PlayerCombatActions.SetPlayerTarget += SetTarget;
        PlayerCombatActions.RemovePlayerTarget += RemoveTarget;
        RemoveTarget += RemoveTargetFromButtons;
    }

    private void Start()
    {
        attackButton.interactable = false;
    }

    private void OnActionButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke(skill);
        RemoveTarget?.Invoke();
    }

    private void SetTarget(Character target)
    {
        attackButton.interactable = true;
        currentTarget = target.gameObject;
    }

    private void RemoveTargetFromButtons()
    {
        attackButton.interactable = false;
        currentTarget = null;
    }

    public void SetScriptableObject(Skill skillScriptableObject)
    {
        skill = skillScriptableObject;
        buttonText.text = skill.SkillName;
    }

    private void BasedOnCombatState(CombatState combatState)
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
        CombatManager.CurrentCombatPhase -= BasedOnCombatState;
        PlayerCombatActions.SetPlayerTarget -= SetTarget;
        PlayerCombatActions.RemovePlayerTarget -= RemoveTarget;
        RemoveTarget += RemoveTargetFromButtons;

    }
}
