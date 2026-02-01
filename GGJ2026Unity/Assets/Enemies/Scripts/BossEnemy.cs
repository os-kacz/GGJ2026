using UnityEngine;

public class BossEnemy : EnemyBaseScript
{
    private AbilityController abilityController;

    override protected void Start()
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
    override public void AttackState()
    {
        playerRb.linearVelocity = new Vector2(0, 0);

        if (isPlayerInRange(attackRange))
        {
            if (!animator.GetBool("IsAttacking"))
            {

                int attackOrAbility = Random.Range(0, 2);

                if (attackOrAbility == 0)
                {
                    // Do normal Attack Sword
                    // abilityController.EnemyAttack();
                }
                else
                {
                    // abilityController.EnemyUseMask();
                    // Debug.Log("abILITY");
                }

                animator.SetBool("IsAttacking", true);

            }

        }
        else
        {

            enemyState = EnemyState.Chase;
        }
    }
}
