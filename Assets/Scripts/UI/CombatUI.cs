using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private GameObject combatButtons;

    private GameObject player;

    private void OnEnable()
    {
        CombatManager.CurrentCombatPhase += SetUI;    
    }

    private void SetUI(CombatState combatState)
    {
       if(combatState == CombatState.Setup)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Character character = player.GetComponent<Character>();
            playerName.text = character.Name;
            playerHealth.text = character.Health.ToString();
            combatButtons.SetActive(true);
        }
    }

    private void OnDisable()
    {
        CombatManager.CurrentCombatPhase -= SetUI;

    }
}
