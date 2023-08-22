using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    public static event Action PlayerAttack;
    public CombatState combatState;


    private void Awake()
    {
        attackButton.onClick.AddListener(OnActionButton);
    }

    private void Update()
    {
        DisableButton();
    }

    private void OnActionButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke();
    }

    private void DisableButton()
    {
        
    }

    private void OnDisable()
    {
        attackButton.onClick.RemoveListener(OnActionButton);
    }
}
