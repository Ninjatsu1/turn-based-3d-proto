using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerCombatActions : MonoBehaviour
{
    public static event Action<bool> PlayerDidAction;

    private void OnEnable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
    }

    private void PlayerCombatAction()
    {
        Debug.Log("Attacking");
        PlayerDidAction?.Invoke(true);
    }


    private void OnDisable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
    }
}
