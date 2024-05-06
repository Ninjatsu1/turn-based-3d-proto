using System;
using UnityEngine;

public class EnemyCombatActions : MonoBehaviour
{
    public static Action<bool> EnemyDidAction;

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform projectileSpawn;
    private Character target;
    private static readonly string playerTag = "Player";
    private Character characterStats;

    private void OnEnable()
    {
        CombatManager.EnemyAction += DoAction;
        Projecetile.ProjectileReachedDestination += ProjectileReachedDestination;
    }

    private void Start()
    {
        GetTarget();
    }

    private void GetTarget() //Implement better method to get player
    {
        target = GameObject.FindWithTag(playerTag).GetComponent<Character>();
        characterStats = GetComponent<Character>();
    }

    private void DoAction(Character character)
    {
        Debug.Log("Attacking: " + target);
        AttackTarget();

    }

    private void ProjectileReachedDestination(Character whoShot, Character target)
    {
        if (!whoShot.IsPlayerCharacter)
        {
            EnemyDidAction?.Invoke(true);
        }
    }

    private void AttackTarget()
    {
        if(target is IDamageable)
        {
            int pickedSkill = PickSkill();
            GameObject objectToSpawn = characterStats.CharacterSkills[pickedSkill].SkillObject.gameObject;
            GameObject projectileToShoot = Instantiate(objectToSpawn, projectileSpawn);
            
            projectileToShoot.GetComponent<Projecetile>().SetTarget(characterStats, target, pickedSkill);
        }
    }

    private int PickSkill()
    {
        int randomSkill = UnityEngine.Random.Range(0, characterStats.CharacterSkills.Count);
        return randomSkill;
    }

    private void OnDisable()
    {
        CombatManager.EnemyAction -= DoAction;
        Projecetile.ProjectileReachedDestination -= ProjectileReachedDestination;
    }
}
