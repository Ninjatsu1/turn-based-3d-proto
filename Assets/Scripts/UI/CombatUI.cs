using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerHealth;
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private GameObject combatButtons;
    [SerializeField]
    private GameObject enemyHealthPanel;
    [SerializeField]
    private TextMeshProUGUI enemyName;
    [SerializeField]
    private TextMeshProUGUI enemyHealth;
    [SerializeField]
    private Slider enemyHealthSlider;

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
