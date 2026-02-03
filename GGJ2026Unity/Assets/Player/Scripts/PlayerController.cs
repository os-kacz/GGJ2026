using System;
using System.Collections;
using TreeEditor;
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
    InventoryController PlayerInventory;

    BoxCollider2D enemyDetectionBox;

    HealthComponent _healthComponent;

    [SerializeField] private Animator _animator;

    private AudioSource _audioSource;

    [SerializeField] AudioClip[] swordSoundEffectArray;
    [SerializeField] AudioClip[] runSoundEffectArray;

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

    public LayerMask wallLayer;
    private bool onWallLeft;
    private bool onWallRight;
    public Transform wallPositionLeft;
    public Transform wallPositionRight;

    public float wallSlideSpeed;

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

        enemyDetectionBox = GetComponent<BoxCollider2D>();

        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.E_EntityHasDied.AddListener(IsDead);

        _audioSource = GetComponent<AudioSource>();
        
        //sets rigidbody
        playerRb = GetComponent<Rigidbody2D>();
        // sets sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
                    
        attackPosition.SetPositionAndRotation(new Vector3(transform.position.x + 0.2f,transform.position.y,0f), new Quaternion(0f,0f,0f,0f));

        abilityController = GetComponent<AbilityController>();
        PlayerInventory.AddMaskToInventory("Ballistics Mask");
        PlayerInventory.AddMaskToInventory("Frozen Mask");
        PlayerInventory.AddMaskToInventory("Fire Oni Mask");
        PlayerInventory.AddMaskToInventory("Swordsman Mastery");
        PlayerInventory.AddMaskToInventory("Void Oni Mask");
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        Jump();
        Sprint();
        Dodge();
        if(timer >= 0.5f)
        {
            Attack();
        }

        Ability1();
        Ability2();


        CheckLeftWall();
        CheckRightWall();


        isGroundFloor = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundFloorLayer);
        isOnEnemy = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, enemyLayer);

        if(attackAction.WasReleasedThisFrame())
        {
            _animator.SetBool("IsAttacking", false);
        }

        if(dodgeAction.WasReleasedThisFrame())
        {
            _animator.SetBool("IsDashing", false);
        }

        if(abilityAction_1.WasReleasedThisFrame())
        {
            _animator.SetBool("IsSlamming", false);
        }

        if(abilityAction_2.WasReleasedThisFrame())
        {
            _animator.SetBool("IsSlamming", false);
        }




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
            _animator.SetBool("IsAttacking", true);
            hitEnemy = Physics2D.OverlapCapsule(attackPosition.position, attackSize, CapsuleDirection2D.Horizontal, 0f, enemyLayer, -1f, 1f);

            StartCoroutine(SwordSoundDelay());

            if(hitEnemy == true)
            {
                
                abilityController.PlayerAttack();
                timer = 0f;
                
                
            }
            
        }  

        
    }

    IEnumerator SwordSoundDelay()
    {
        yield return new WaitForSeconds(0.15f);
        if(swordSoundEffectArray.Length != 0)
        {
            int attackChosen = UnityEngine.Random.Range(0, swordSoundEffectArray.Length -1);
            _audioSource.PlayOneShot(swordSoundEffectArray[attackChosen]);
                          
        }
        Debug.Log("Attack Sound");
        
    }

    void Ability1()
    {
        if(abilityAction_1.WasPressedThisFrame())
        {
            // abilityController.TriggerAbility1();

            NewMask.AnimationState AnimID = abilityController.TriggerAbility1();
            switch(AnimID)
            {
                case NewMask.AnimationState.Slam:
                _animator.SetBool("IsSlamming", true);
                break;
            }

        }
        
    }

    void Ability2()
    {
        if(abilityAction_2.WasPressedThisFrame())
        {
            NewMask.AnimationState AnimID = abilityController.TriggerAbility2();
            switch(AnimID)
            {
                case NewMask.AnimationState.Slam:
                _animator.SetBool("IsSlamming", true);
                break;
            }
        }
    }

    void Dodge()
    {
        if(dodgeAction.WasPressedThisFrame())
        {
            if(playerRb.linearVelocityY == 0)
            {
                if(moveValue.x > 0)
                {
                    this.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
                }
                else if(moveValue.x < 0)
                {
                    this.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z);
                }
                else if(moveValue.x == 0)
                {
                    if(spriteRenderer.flipX == true)
                    {
                        this.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y, transform.position.z);
                    }
                    else
                    {
                        this.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y, transform.position.z);
                    }
                }
                
                _animator.SetBool("IsDashing", true);
            }

        } 
    }

    void CheckLeftWall()
    {
        onWallLeft = Physics2D.OverlapCircle(wallPositionLeft.position, groundCheckCircle, wallLayer);
        if(onWallLeft == true && !isGrounded && !isGroundFloor && moveValue.x != 0)
        {
            _animator.SetBool("LeftSlide", true);
            this.transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y - wallSlideSpeed), transform.position.z);

        }
        else
        {
            _animator.SetBool("LeftSlide", false);
        }
    }

    void CheckRightWall()
    {
        onWallRight = Physics2D.OverlapCircle(wallPositionRight.position, groundCheckCircle, wallLayer);
        if(onWallRight == true && !isGrounded && !isGroundFloor && moveValue.x != 0)
        {
            _animator.SetBool("RightSlide", true);
            this.transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y - wallSlideSpeed), transform.position.z);

            //playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, Mathf.Max(playerRb.linearVelocityY - wallSlideSpeed));

        }
        else
        {
            _animator.SetBool("RightSlide", false);
        }
    }


    void IsDead()
    {
        _animator.SetBool("IsDead", true);
    }
}
