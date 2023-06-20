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

        // when the jump button in input control is called, call Jump()
        inputControl = new InputControls();
        inputControl.Gameplay.Jump.started += Jump;

        isRunning = false;
    }

    // Most physics related function in Fixed Update
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

    #region Movement Related Functions
    /**
        void Movement()
        Input: void
        Output: void
        Function:
            - move players along x-axis with speed
     */
    private void Movement()
    {
        // Move player
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        // Flip sprite based on moving direction
        FlipSprite();
    }

    private void FlipSprite()
    {
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }
    #endregion

    /**
    void Jump()
    Input: void
    Output: callback
    Function:
        - Be called when the space is pressed
        - Add force to allow player jump
    */
    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    #region Enable and Disable Input System
    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    #endregion

    // Function used to visualize the area of checking whether player is standing on ground
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    // SetAnimation: change animation based on the player info on rigid body
    private void SetAnimation()
    {
        playerAnimator.SetFloat("vX", MathF.Abs(rb.velocity.x));
        playerAnimator.SetFloat("vY", rb.velocity.y);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }
}
