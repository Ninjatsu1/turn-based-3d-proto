using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerHealth;
    [SerializeField]
    private GameObject combatButtons;
    [SerializeField]
    private TextMeshProUGUI enemyName;
    [SerializeField]
    private TextMeshProUGUI enemyHealth;
    private GameObject player;

    private void OnEnable()
    {
        CombatManager.CurrentCombatPhase += SetUI;
        Character.CharacterCurrentHealth += UpdateHealth;
        PlayerCombatActions.SetPlayerTarget += DisplayEnemyHealth;
        PlayerCombatActions.RemovePlayerTarget += OnDeselectEnemy;
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
       if(combatState == CombatState.Win)
        {
            OnDeselectEnemy();
        }
    }

    private void UpdateHealth(Character character)
    {
        if (character.IsPlayerCharacter)
        {
            playerHealth.text = character.CurrentHealh.ToString();
        } else
        {
            if(enemyName.text == character.Name)
            {
                enemyHealth.text = character.CurrentHealh.ToString();
            }
        }
    }

    private void DisplayEnemyHealth(Character character)
    {
        if(!character.IsPlayerCharacter)
        {
            enemyName.text = character.Name;
            enemyHealth.text = character.CurrentHealh.ToString();
            enemyHealth.gameObject.SetActive(true);
            enemyName.gameObject.SetActive(true);
        }
    }

    private void OnDeselectEnemy()
    {
        enemyHealth.gameObject.SetActive(false);
        enemyName.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CombatManager.CurrentCombatPhase -= SetUI;
        Character.CharacterCurrentHealth -= UpdateHealth;
        PlayerCombatActions.SetPlayerTarget -= DisplayEnemyHealth;
        PlayerCombatActions.RemovePlayerTarget -= OnDeselectEnemy;
    }
}
