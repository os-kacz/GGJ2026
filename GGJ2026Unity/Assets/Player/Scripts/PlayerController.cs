using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    Rigidbody2D playerRb;
    SpriteRenderer spriteRenderer;

    public float speed;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        if ( moveValue.x < 0)
        {
            spriteRenderer.flipX = true;

        }
        else if (moveValue.x > 0)
        {
            spriteRenderer.flipX = false;

        }
        playerRb.linearVelocity = new Vector2(moveValue.x * speed, playerRb.linearVelocityY);

    }

}
