using UnityEngine;

public class BossEnemy : EnemyBaseScript
{
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
                    abilityController.EnemyAttack();
                    StartCoroutine(soundArray());
                }
                else
                {
                    abilityController.EnemyUseMask();
                    Debug.Log("abILITY");
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
