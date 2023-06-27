using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    #region All Variables
    // Basic Variables
    private Rigidbody2D rb;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

    // Input Variables
    private InputControls inputControl;
    private Vector2 inputDirection;

    // Design Variables
    [Header("Basic Variables")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    // Ground Check Variables
    [SerializeField]
    private float checkRadius;
    [SerializeField]
    private LayerMask groundLayer;

    // Player Status Variables
    private bool isFastRun;
    private bool isDoubleJumped;
    private bool isGrounded;
    private bool isKnockBack;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        // when the jump button in input control is called, call Jump()
        inputControl = new InputControls();
        inputControl.Gameplay.Jump.started += Jump;
    }

    // Most physics related function in Fixed Update
    private void FixedUpdate()
    {
        if (!isKnockBack)
            Movement();
    }

    private void Update()
    {
        // Ground Check about whether player is standing on ground
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

        // Read player input direction
        inputDirection = inputControl.Gameplay.Movement.ReadValue<Vector2>();

        // Set animation based on input
        SetAnimation();

        // Flip sprite based on moving direction
        FlipSprite();

        // When land on the ground, reset isKnockBack and isDoubleJumped
        GroundResetStatus();

        // Check player input of fast run
        FastRunCheck();
    }

    private void GroundResetStatus()
    {
        if (isGrounded)
        {
            isKnockBack = false;
            isDoubleJumped = false;
        }
    }

    #region Movement Related Functions
    /**
        void Movement()
        Input: void
        Output: void
        Function:
            - Run : move player along x-axis with speed
            - Fast Run: run with double speed
     */
    private void Movement()
    {
        // Move player
        if (isFastRun)
            rb.velocity = new Vector2(inputDirection.x * 2 * speed * Time.deltaTime, rb.velocity.y);
        else
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
    }
    #endregion

    #region Jump Related Functions
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
        {
            //rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (!isDoubleJumped)
        {
            //rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isDoubleJumped = true;
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    #endregion

    #region Fast Run Related Functions
    private void FastRunCheck()
    {
        if (inputControl.Gameplay.FastRun.IsPressed())
            isFastRun = true;
        else
            isFastRun = false;
    }
    #endregion

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

    #region Trap Collision & KnockBack Functions
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Trap")
        {
            Debug.Log("hurt");
            isKnockBack = true;
            //rb.velocity = new Vector2(-rb.velocity.normalized.x * knockbackForce, 100);
        }
    }
    #endregion

    #region Player Animation and Sprite Related Functions
    // SetAnimation: change animation based on the player info on rigid body
    private void SetAnimation()
    {
        playerAnimator.SetFloat("vX", MathF.Abs(rb.velocity.x));
        playerAnimator.SetFloat("vY", rb.velocity.y);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetBool("isHit", isKnockBack);
    }

    // FlipSprite: Flip player's sprite based on the moving direction
    private void FlipSprite()
    {
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }
    #endregion

    // Function used to visualize the area of checking whether player is standing on ground
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
