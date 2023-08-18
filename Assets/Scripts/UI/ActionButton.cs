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
        attackButton.onClick.AddListener(OnActionButton);
    }

    private void OnActionButton()
    {
        Debug.Log("button pressed");
        PlayerAttack?.Invoke();
    }
}
