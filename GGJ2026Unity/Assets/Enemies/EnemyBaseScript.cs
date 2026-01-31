using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class EnemyBaseScript : MonoBehaviour
{
    private enum EnemyState
    {
        Idle = 0,
        Patrol = 1,
        Chase = 2,
        Attacking = 3,
        Dead = 4
    }


    // Physics and Controls
    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;
    HealthComponent healthComponent;

    // Basic enemy stats and variables

    [Header("Base stats")]

    public string enemyName = "Enemy";

    public float damageMultiplier = 1f;

    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    private float currentSpeed = 0f;
    public float attackSpeedMultiplier = 1f;

    public float attackTime = 1f;
    private float attackTimer = 0f;

    // AI
    [Header("AI")]

    EnemyState enemyState = EnemyState.Idle;

    private Vector3 startLocation;
    private Vector3 targetLocation;

    private GameObject targetPlayer;

    private int lookingAtDirection = 1;

    // Ranges
    [Header("Ranges")]

    public float patrolRange = 10f;
    public float detectionRange = 4f;
    public float attackRange = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {

        startLocation = gameObject.transform.position;

        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthComponent = GetComponent<HealthComponent>();

        targetPlayer = GameObject.FindGameObjectWithTag("Player");

        currentSpeed = walkSpeed;

        attackTimer = attackTime;
        healthComponent.E_EntityHasDied.AddListener(DeathState);


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

                 currentSpeed = walkSpeed;
                 patrolState();

                 break;

             case EnemyState.Chase:

                 currentSpeed = runSpeed;
                 chaseState();

                 break;

             case EnemyState.Attacking:

                 AttackState();
                 break;

             case EnemyState.Dead:

                break;

         }
      
        
        
    }

    public void DeathState()
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

        if (isPlayerInRange(detectionRange))
        {
            enemyState = EnemyState.Chase;
            return;
        }

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

        return;

    }

    public void chaseState()
    {
        // What should the enemy do when they have seen the player?

        // check if in detection range

        if (!isPlayerInRange(detectionRange))
        {
            // if no change state to patrol and return

            enemyState = EnemyState.Patrol;
            CalculateNewTargetPosition();
            return;
        }

        // check if player is in the attack range

        if (isPlayerInRange(attackRange))
        {
            // if yes attack

            enemyState = EnemyState.Attacking;
        }
        else
        {
            // if no move to the enemy
            targetLocation = targetPlayer.transform.position;
            MoveTo();
        }


    }

    public bool isPlayerInRange(float range)
    {

        switch (lookingAtDirection)
        {
            case -1:
                if (gameObject.transform.position.x - range < targetPlayer.transform.position.x && targetPlayer.transform.position.x < gameObject.transform.position.x)
                {
                    return true;
                }
                break;

            case 1:
                if (gameObject.transform.position.x + range > targetPlayer.transform.position.x && targetPlayer.transform.position.x > gameObject.transform.position.x)
                {
                    return true;
                }
                break;
        }
        return false;
    }

    public void AttackState()
    {
        attackTimer += Time.deltaTime;

        playerRb.linearVelocity = new Vector2(0, 0);
        spriteRenderer.color = Color.blue;

        if (isPlayerInRange(attackRange))
        {
            if (attackTimer >= attackTime)
            {
                Debug.Log("Attack");
                attackTimer = 0f;
            }
        }
        else
        {
            spriteRenderer.color = Color.red;

            enemyState = EnemyState.Chase;
        }
        
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

        playerRb.linearVelocity = new Vector2(lookingAtDirection * currentSpeed, playerRb.linearVelocityY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!healthComponent.isDead && collision.gameObject.CompareTag("Player"))
        {
            healthComponent.DecreaseHealthBy(10);
        }
    }
}


