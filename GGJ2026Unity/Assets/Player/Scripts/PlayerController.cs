using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Input actions
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction interactAction;
    InputAction abilityAction_1;
    InputAction abilityAction_2;
    InputAction dodgeAction;
    InputAction sprintActoin;
    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;
    AbilityController abilityController;

    HealthComponent _healthComponent;

    [SerializeField] private Animator _animator;

    // movement speed and jump force for player
    private float speed;
    public float baseSpeed;
    public float sprintSpeed;
    public float jumpForce;

    // jump variables for single jump and improvements
    public LayerMask groundLayer;
    public LayerMask groundFloorLayer;
    private bool isGrounded;
    private bool isGroundFloor;
    private bool isOnEnemy;
    public Transform feetPosition;
    public float groundCheckCircle;

    private Collider2D currentFloor;

    private bool hitEnemy;
    public Transform attackPosition;
    public Vector2 attackSize;
    public LayerMask enemyLayer;

    private Vector2 moveValue;

    float timer = 1f;



    void Start()
    {
        //sets actions from the input system
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        interactAction = InputSystem.actions.FindAction("Interact");
        abilityAction_1 = InputSystem.actions.FindAction("Ability1");
        abilityAction_2 = InputSystem.actions.FindAction("Ability2");
        dodgeAction = InputSystem.actions.FindAction("Dodge");
        sprintActoin = InputSystem.actions.FindAction("Sprint");

        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.E_EntityHasDied.AddListener(IsDead);
        //sets rigidbody
        playerRb = GetComponent<Rigidbody2D>();
        // sets sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
                    
        attackPosition.SetPositionAndRotation(new Vector3(transform.position.x + 0.2f,transform.position.y,0f), new Quaternion(0f,0f,0f,0f));

        abilityController = GetComponent<AbilityController>();
        abilityController.CollectMask("Swordsman Mastery", 1);
        abilityController.CollectMask("Frozen Mask", 2);
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        Jump();
        Sprint();
        if(timer >= 0.5f)
        {
            Attack();
        }
        
        if(abilityAction_1.WasPressedThisFrame())
        {
            abilityController.TriggerAbility1();
        }

         if(abilityAction_2.WasPressedThisFrame())
        {
            abilityController.TriggerAbility2();
        }

        isGroundFloor = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundFloorLayer);
        isOnEnemy = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, enemyLayer);

    }

    void FixedUpdate()
    {
        //gets the value from movement input
        moveValue = moveAction.ReadValue<Vector2>();

        // checks which direction player is facing a flips the spirte
        if ( moveValue.x < 0)
        {
            spriteRenderer.flipX = true;
            attackPosition.SetPositionAndRotation(new Vector3(transform.position.x - 0.2f,transform.position.y,0f), new Quaternion(0f,0f,0f,0f));

        }
        else if (moveValue.x > 0)
        {
            spriteRenderer.flipX = false;
            attackPosition.SetPositionAndRotation(new Vector3(transform.position.x + 0.2f,transform.position.y,0f), new Quaternion(0f,0f,0f,0f));

        }

        //Move down through floors
        if(moveValue.y < 0)
        {
            if(isGroundFloor == true && isOnEnemy == false)
            {
                StartCoroutine(FallTimer());                
            }

        }

        // player movement
 
        playerRb.linearVelocity = new Vector2(moveValue.x * speed, playerRb.linearVelocityY);
        _animator.SetFloat("xVelocity", Math.Abs(playerRb.linearVelocityX));
        _animator.SetFloat("yVelocity", playerRb.linearVelocityY);


        if(playerRb.linearVelocityY != 0)
        {
            _animator.SetBool("IsJumping", true);
        }
        else
        {
            _animator.SetBool("IsJumping", false);

        }
    }

    void Sprint()
    {
        if (sprintActoin.IsPressed() && isGrounded == true)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = baseSpeed;
        }
    }

    void Jump()
    {
        /* checks if the player is grounded 
        First creates invisibl circle, 
        put it at the players feet, 
        make it the same size as specified,
        check if the circle overlaps the ground*/
        
        
        // player jump
        if (isGrounded == true || isGroundFloor == true)
        {
            if(jumpAction.WasPressedThisFrame())
            {
                playerRb.linearVelocity = Vector2.up * jumpForce;
            }
            
        }
    }

    IEnumerator FallTimer()
    {
        //removes collider for a small time
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        this.GetComponent<CapsuleCollider2D>().enabled = true;      

    }

    void Attack()
    {
        if(attackAction.WasPressedThisFrame())
        {
            hitEnemy = Physics2D.OverlapCapsule(attackPosition.position, attackSize, CapsuleDirection2D.Horizontal, 0f, enemyLayer, -1f, 1f);
            
            if(hitEnemy == true)
            {
                Debug.Log("Hit");
                abilityController.PlayerAttack();
                timer = 0f;
                
                //add ability.baseAttack function
            }
            
        }  
    }

    void Ability1()
    {
        
    }

    void Ability2()
    {
        
    }
    void IsDead()
    {
        _animator.SetBool("IsDead", true);
    }
}
