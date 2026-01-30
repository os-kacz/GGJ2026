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

    }

    public void patrolState()
    {
        // What should the enemy do, walk around, stay still?

    }

    public void chaseState()
    {
       // What should the enemy do when they have seen an enemy?


    }
}
