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


    // Physics and Controls

    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;

    // Basic enemy stats and variables

    public float maxHealth = 100;
    private float currentHealth = 100;

    public float damageMultiplier = 1;

    public float movementSpeed = 1;
    public float attackSpeedMultiplier = 1;

    EnemyState enemyState = EnemyState.Idle;

    public Vector3 startLocation;
    public Vector3 targetLocation;

    private int lookingAtDirection = 1;

    public float patrolRange = 25;
    public float detectionRange = 20;
    public float attackRange = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentHealth = maxHealth;

        startLocation = gameObject.transform.position;

        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        enemyState = EnemyState.Patrol;
        CalculateNewTargetPosition();
    }

    public void patrolState()
    {
        // What should the enemy do, walk around, stay still?

        // check if player is in detection range

        // if true, move to attack state

        // else

        // check if the enemy has a target location

        if (targetLocation == null)
        {
            CalculateNewTargetPosition();
        }

        // check if player is in sight


        if (gameObject.transform.position.x <= targetLocation.x + 1 && gameObject.transform.position.x >= targetLocation.x - 1)
        {
            // if reached target location, 

            enemyState = EnemyState.Idle;
        }
        else
        {
            // move to target location
            MoveTo();
            
        }



    }

    public void chaseState()
    {
       // What should the enemy do when they have seen an enemy?

        // check if in detection range

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

    public void CalculateNewTargetPosition()
    {
        targetLocation.x = Random.Range(startLocation.x - patrolRange, startLocation.x + patrolRange);

        return;
    }

    public void MoveTo()
    {
        if (lookingAtDirection > 0)
        {
            if (targetLocation.x - gameObject.transform.position.x < 0)
            {
                lookingAtDirection = -1;
                spriteRenderer.flipX = true;
            }
        }
        else if (lookingAtDirection < 0)
        {
            if (targetLocation.x - gameObject.transform.position.x > 0)
            {
                lookingAtDirection = 1;
                spriteRenderer.flipX = false;
            }
        }

        playerRb.linearVelocity = new Vector2(lookingAtDirection * movementSpeed, playerRb.linearVelocityY);
    }
}
