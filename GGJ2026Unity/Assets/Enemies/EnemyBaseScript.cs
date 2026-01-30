using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    private enum EnemyState
    {
        Idle = 0,
        Patrol = 1,
        Chase = 2,
        Dead = 3
    }

    // Basic enemy stats and variables

    public float maxHealth = 100;
    private float currentHealth = 100;

    public float damageMultiplier = 1;

    public float movementSpeed = 1;
    public float attackSpeedMultiplier = 1;

    EnemyState enemyState = EnemyState.Idle;

    public float detectionRange = 20;
    public float attackRange = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    protected void Update()
    {
        // state tree
        switch (enemyState)
        {
           case EnemyState.Idle:

                IdleState();

                break;
           
           case EnemyState.Patrol:

                patrolState();

                break;

           case EnemyState.Chase:

                chaseState();

                break;

           case EnemyState.Dead:

                onDeath();

                break;
        }
    }

    public void TakeDamage(float damage)
    {
        // damage functions
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            onDeath();
        }
    }

    public void onDeath()
    {
        // vfx, sfx and other daeth additions
        Destroy(gameObject);
    }

    public void IdleState()
    {
        // Idle animation / features

        // maybe move to patrol state after a second?

    }

    public void patrolState()
    {
        // What should the enemy do, walk around, stay still?

        // check if player is in detection range

        // if true, move to attack state

        // else
        // check if the enemy has a target location
        // move to target location

        // if reached target location, 
        

    }

    public void chaseState()
    {
       // What should the enemy do when they have seen an enemy?

        // check ig in detection range

        // if no change state to patrol and return

        // create new target location

        // check if player is in the attack range

        // if yes attack

        // if no move to the enemy
    }

    public void isPlayerInRange(float range)
    {

        // check if the player is in range of the enemy (allow for any range to be entered e.g. attack and detection)

        // get player location, get enemy location, find distance between them

        // return yes, if its less than the range
    }

    public void Attack()
    {

    }
}
