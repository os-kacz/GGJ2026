using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Input actions
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;

    // movement speed and jump force for player
    public float speed;
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
        //sets rigidbody
        playerRb = GetComponent<Rigidbody2D>();
        // sets sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
             // player jump
        if (isGrounded == true && jumpAction.WasPressedThisFrame())
        {
            playerRb.linearVelocity = Vector2.up * jumpForce;
        }

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

        // player movement
        playerRb.linearVelocity = new Vector2(moveValue.x * speed, playerRb.linearVelocityY);

        /* checks if the player is grounded 
            First creates invisibl circle, 
            put it at the players feet, 
            make it the same size as specified,
            check if the circle overlaps the ground*/
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);


    }

}
