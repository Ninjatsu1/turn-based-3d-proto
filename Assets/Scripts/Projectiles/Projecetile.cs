using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projecetile : MonoBehaviour
{
    public static event Action<Character, Character> ProjectileReachedDestination;
    private int projectileDamage;

    private Character originalShooter;
    private Character target;
    private bool hasTarget = false;

    public void SetTarget(Character whoShot, Character targetToReach, int damage)
    {
        Debug.Log("Target: " + target);
        hasTarget = true;
        originalShooter = whoShot;
        target = targetToReach;
        projectileDamage = damage;
    }

    private void Update()
    {
        if(hasTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 10f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;
        if(collidedObject.GetComponent<Character>() != null)
        {
            ProjectileReachedDestination?.Invoke(originalShooter, target);
            target.Damage(projectileDamage);
            Debug.Log("Damagable!");
            Destroy(gameObject);
        } else
        {
            Debug.Log("Not damagable!");
            Destroy(gameObject);
        }
    }
}
