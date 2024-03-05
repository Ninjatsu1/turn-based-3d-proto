using System;
using UnityEngine;

public class EnemyCombatActions : MonoBehaviour
{
    private Character target;
    private static readonly string playerTag = "Player";
    private Character characterStats;
    public static Action<bool> EnemyDidAction;

    private void OnEnable()
    {
        CombatManager.EnemyAction += DoAction;
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
        EnemyDidAction?.Invoke(true);
    }

    private void AttackTarget()
    {
        if(target is IDamageable)
        {
           Character targetCharacter = target.GetComponent<Character>();
           targetCharacter.Damage(characterStats.Attack);
        }
    }

    private void OnDisable()
    {
        CombatManager.EnemyAction -= DoAction;

    }
}
