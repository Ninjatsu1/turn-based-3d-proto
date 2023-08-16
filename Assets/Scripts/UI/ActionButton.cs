using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private Button attackButton;
    public static event Action PlayerAttack;

    private void Start()
    {
        attackButton.onClick.AddListener(OnAttackButton);
    }

    private void OnAttackButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke();
    }
}
