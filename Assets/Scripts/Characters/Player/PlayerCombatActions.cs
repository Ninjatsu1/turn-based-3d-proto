using UnityEngine;
using System;

public class PlayerCombatActions : MonoBehaviour
{
    public static event Action<bool> PlayerDidAction;
    public static event Action<Character> SetPlayerTarget;
    public static event Action RemovePlayerTarget;

    [SerializeField]
    private Transform projectileSpawn;
    [SerializeField]
    private GameObject projectile; //Possible refactor later

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
        Projecetile.ProjectileReachedDestination += ProjectileReachedDestination;
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

    private void PlayerCombatAction(Skill skill)
    {
        Character target = playerTargetObject.GetComponent<Character>();
        if(target is IDamageable)
        {
            GameObject projectileToShoot = Instantiate(skill.SkillObject.gameObject, projectileSpawn);
            projectileToShoot.GetComponent<Projecetile>().SetTarget(playerStats, target, skill.DamageAmount);
        }
    }

    private void ProjectileReachedDestination(Character whoShot, Character target)
    {
        if(whoShot.IsPlayerCharacter)
        {
            PlayerDidAction?.Invoke(true);
        } 
    }

    private void OnDisable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
        Character.CharacterEliminated -= DeselectEnemy;
        Projecetile.ProjectileReachedDestination -= ProjectileReachedDestination;
    }
}
