using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CombatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerHealth;
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private List<Button> combatButtons = new List<Button>();
    [SerializeField]
    private GameObject enemyHealthPanel;
    [SerializeField]
    private TextMeshProUGUI enemyName;
    [SerializeField]
    private TextMeshProUGUI enemyHealth;
    [SerializeField]
    private Slider enemyHealthSlider;
    [SerializeField]
    private GameObject footer;

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
            playerHealth.text = character.MaxHealth.ToString();
            playerHealthSlider.maxValue = character.MaxHealth;
            playerHealthSlider.value = character.CurrentHealh;
            GetButtons(character);
            DisplayButtons();
            //combatButtons.SetActive(true);
            
        }
       if(combatState == CombatState.Win)
        {
            OnDeselectEnemy();
        }
    }

    private void GetButtons(Character character)
    {
        for (int i = 0; i < character.CharacterStats.Skills.Count; i++)
        {
            Button button = Instantiate(character.CharacterStats.Skills[i].SkillButton);
            button.transform.parent = footer.transform;
        }
    }

    private void DisplayButtons()
    {
        for (int i = 0; i < footer.transform.childCount; i++)
        {
            footer.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void UpdateHealth(Character character)
    {
        if (character.IsPlayerCharacter)
        {
            playerHealth.text = character.CurrentHealh.ToString();
            playerHealthSlider.value = character.CurrentHealh;
        } else
        {
            if(enemyName.text == character.Name)
            {
                enemyHealth.text = character.CurrentHealh.ToString();
                enemyHealthSlider.maxValue = character.MaxHealth;
                enemyHealthSlider.value = character.CurrentHealh;
            }
        }
    }

    private void DisplayEnemyHealth(Character character)
    {
        if(!character.IsPlayerCharacter)
        {
            enemyName.text = character.Name;
            enemyHealth.text = character.CurrentHealh.ToString();
            enemyHealthSlider.maxValue = character.MaxHealth;
            enemyHealthSlider.value = character.CurrentHealh;
            enemyHealthPanel.SetActive(true);
        }
    }

    private void OnDeselectEnemy()
    {
        enemyHealthPanel.SetActive(false);
    }

    private void OnDisable()
    {
        CombatManager.CurrentCombatPhase -= SetUI;
        Character.CharacterCurrentHealth -= UpdateHealth;
        PlayerCombatActions.SetPlayerTarget -= DisplayEnemyHealth;
        PlayerCombatActions.RemovePlayerTarget -= OnDeselectEnemy;
    }
}
