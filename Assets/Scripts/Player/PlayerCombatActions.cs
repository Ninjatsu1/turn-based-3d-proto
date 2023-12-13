using UnityEngine;
using System;

public class PlayerCombatActions : MonoBehaviour
{
    public static event Action<bool> PlayerDidAction;
    public static event Action<Character> SetPlayerTarget;
    public static event Action RemovePlayerTarget;
    private GameObject playerTargetObject = null;
    
    private readonly string enemyTag = "Enemy";
    private Character playerStats;

    private void Awake()
    {
        playerStats = GetComponent<Character>();
    }

    private void OnEnable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
        Character.CharacterEliminated += DeselectEnemy;
    }

    private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
				Debug.Log("Hit " + hitInfo.transform.gameObject.name);
				if (hitInfo.transform.gameObject.tag == enemyTag)
				{
                    playerTargetObject = hitInfo.transform.gameObject;
                    SetPlayerTarget?.Invoke(playerTargetObject.GetComponent<Character>());
                }
			}
		}
        if(Input.GetMouseButtonDown(1))
        {
            playerTargetObject = null;
            RemovePlayerTarget?.Invoke();
        }
	}

    private void DeselectEnemy(Character character)
    {
        if(!character.IsPlayerCharacter)
        {
           Character selectedTarget = playerTargetObject.GetComponent<Character>();
            if(selectedTarget == character)
            {
                playerTargetObject = null;                
            }
        }
    }

    private void PlayerCombatAction()
    {
        Debug.Log("Attacking");
        Character target = playerTargetObject.GetComponent<Character>();
        if(target is IDamageable)
        {
            target.Damage(playerStats.Attack);
        }
        PlayerDidAction?.Invoke(true);
    }

    private void OnDisable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
        Character.CharacterEliminated -= DeselectEnemy;

    }
}
