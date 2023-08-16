using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerHealth;
    [SerializeField] private GameObject combatButtons;

    private bool textSet = false;
    private GameObject player;

    private void OnEnable()
    {
        CombatManager.CombatStarts += SetUI;    
    }

    private void SetUI()
    {
       player = GameObject.FindGameObjectWithTag("Player");
       Character character = player.GetComponent<Character>();
       playerName.text = character.Name;
       playerHealth.text = character.Health.ToString();
        combatButtons.SetActive(true);
    }


    private void OnDisable()
    {
        CombatManager.CombatStarts -= SetUI;

    }
}
