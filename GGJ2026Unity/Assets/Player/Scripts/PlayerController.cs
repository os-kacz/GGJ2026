using System.Collections;
using Unity.VisualScripting;
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

    // movement speed and jump force for player
    private float speed;
    public float jumpForce;

    // jump variables for single jump and improvements
    public LayerMask groundLayer;
    private bool isGrounded;
    public Transform feetPosition;
    public float groundCheckCircle;

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
        //sets rigidbody
        playerRb = GetComponent<Rigidbody2D>();
        // sets sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Jump();
        Sprint();

    }

    void FixedUpdate()
    {
        //gets the value from movement input
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        // checks which direction player is facing a flips the spirte
        if ( moveValue.x < 0)
        {
            spriteRenderer.flipX = true;

        }
        else if (moveValue.x > 0)
        {
            spriteRenderer.flipX = false;

        }

        //Move down through floors
        if(moveValue.y < 0 && isGrounded == true)
        {
            StartCoroutine(FallTimer());
        }

        // player movement
 
        playerRb.linearVelocity = new Vector2(moveValue.x * speed, playerRb.linearVelocityY);

    }

    void Sprint()
    {
        if (sprintActoin.IsPressed() && isGrounded == true)
        {
            speed = 15;
        }
        else
        {
            speed = 10;
        }
    }

    void Jump()
    {
        /* checks if the player is grounded 
        First creates invisibl circle, 
        put it at the players feet, 
        make it the same size as specified,
        check if the circle overlaps the ground*/
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);
        
        // player jump
        if (isGrounded == true && jumpAction.WasPressedThisFrame())
        {
            playerRb.linearVelocity = Vector2.up * jumpForce;
        }
    }

    IEnumerator FallTimer()
    {
        Debug.Log("returned");
        //removes collider for a small time
        this.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.15f);
        this.GetComponent<BoxCollider2D>().enabled = true;
        

    }

}
