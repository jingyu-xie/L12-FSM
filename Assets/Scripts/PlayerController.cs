using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Basic Variables
    private Rigidbody2D rb;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

    // Input Variables
    public InputControls inputControl;
    public Vector2 inputDirection;

    // Design Variables
    [Header("Basic Variables")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isGrounded;

    private bool isRunning;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        inputControl = new InputControls();
        inputControl.Gameplay.Jump.started += Jump;

        isRunning = false;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void FixedUpdate()
    {
        Movement();
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Movement.ReadValue<Vector2>();
        SetAnimation();
    }

    private void Movement()
    {
        // Move player
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        // Flip sprite based on moving direction
        if (inputDirection.x > 0)
                spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private void SetAnimation()
    {
        playerAnimator.SetFloat("vX", MathF.Abs(rb.velocity.x));
        playerAnimator.SetFloat("vY", rb.velocity.y);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }
}
