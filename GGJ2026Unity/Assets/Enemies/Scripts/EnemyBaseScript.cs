using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class EnemyBaseScript : MonoBehaviour
{
    protected enum EnemyState
    {
        Idle = 0,
        Patrol = 1,
        Chase = 2,
        Attacking = 3,
        Dead = 4
    }


    // Physics and Controls
    protected Rigidbody2D playerRb;
    protected SpriteRenderer spriteRenderer;
    protected HealthComponent healthComponent;
    protected Animator animator;
    private AbilityController abilityController;


    // Basic enemy stats and variables

    [Header("Base stats")]

    public string enemyName = "Enemy";

    public float damageMultiplier = 1f;

    public float speed = 4f;

    protected float currentSpeed = 0f;
    public float attackSpeedMultiplier = 1f;

    public float idleTime = 1f;
    protected float idleTimer = 0f;

    // AI
    [Header("AI")]

    protected EnemyState enemyState = EnemyState.Idle;

    protected Vector3 startLocation;
    protected Vector3 targetLocation;

    protected GameObject targetPlayer;

    protected int lookingAtDirection = 1;

    // Ranges
    [Header("Ranges")]

    public float patrolRange = 10f;
    public float detectionRange = 4f;
    public float attackRange = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

        startLocation = gameObject.transform.position;

        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthComponent = GetComponent<HealthComponent>();

        targetPlayer = GameObject.FindGameObjectWithTag("Player");

        currentSpeed = speed;

        healthComponent.E_EntityHasDied.AddListener(DeathState);

        animator = GetComponent<Animator>();
        abilityController = GetComponent<AbilityController>();

    }

    // Update is called once per frame
    protected void Update()
    {
        // state tree
       
       switch (enemyState)
       {
           case EnemyState.Idle:

               idleTimer += Time.deltaTime;
               IdleState();

               break;

           case EnemyState.Patrol:

               patrolState();

               break;

           case EnemyState.Chase:

               chaseState();

               break;

           case EnemyState.Attacking:

               AttackState();
               break;

           case EnemyState.Dead:


               if (animator.GetBool("CanBeDestroyed"))
               {
                   Destroy(gameObject);
               }

               break;

       }
       
         
    }

    public void DeathState()
    {
        // vfx, sfx and other death additions
        animator.SetBool("IsDead", true);
        animator.SetBool("HasBeenHit", false);

        enemyState = EnemyState.Dead;
        animator.SetFloat("VelocityX", -1);
    }

    public void IdleState()
    {
        // Idle animation / features

        // maybe move to patrol state after a second?
        if (isPlayerInRange(detectionRange))
        {
            enemyState = EnemyState.Chase;
        }
        else
        {
            if (idleTimer >= idleTime)
            {
                enemyState = EnemyState.Patrol;
                CalculateNewTargetPosition();
                idleTimer = 0f;
            }
            MoveTo(0);
        }
            
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
            idleTimer = 0f;
            enemyState = EnemyState.Idle;
        }
        else
        {
            // move to target location
            MoveTo(lookingAtDirection);
            
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
            MoveTo(lookingAtDirection);
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

    public virtual void AttackState()
    {

        playerRb.linearVelocity = new Vector2(0, 0);

        if (isPlayerInRange(attackRange))
        {
            if (!animator.GetBool("IsAttacking"))
            {
                abilityController.EnemyAttack();
                animator.SetBool("IsAttacking", true);
            }

        }
        else
        {

            enemyState = EnemyState.Chase;
        }
        
    }

    public void CalculateNewTargetPosition()
    {
        targetLocation.x = UnityEngine.Random.Range(startLocation.x - patrolRange, startLocation.x + patrolRange);

        return;
    }

    public void MoveTo(float direction)
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

        playerRb.linearVelocity = new Vector2(direction * currentSpeed, playerRb.linearVelocityY);

        animator.SetFloat("VelocityX",  Math.Abs(playerRb.linearVelocityX));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!healthComponent.isDead && collision.gameObject.CompareTag("Player") && !animator.GetBool("HasBeenHit"))
        {
            healthComponent.DecreaseHealthBy(10);
            animator.SetBool("HasBeenHit", true);
        }
    }
}


