using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM : MonoBehaviour
{
    #region All Variables
    // State Related Variables
    private enum PlayerState { Idle, Run, Jump}
    private PlayerState pstate;

    // Component Variables
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
    #endregion

    private void Awake()
    {
        // Get components that will be used
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();

        // Initialize the state
        pstate = PlayerState.Idle;

        // Initialize the input control system
        inputControl = new InputControls();
    }

    private void Update()
    {
        // Read the direction of movement from input
        inputDirection = inputControl.Gameplay.Movement.ReadValue<Vector2>();

        // Function to switch animation
        SetAnimation();

        // FSM
        switch (pstate)
        {
            case PlayerState.Idle:
                IdleActions();
                IdleTransitions();
                break;

            case PlayerState.Run:
                RunActions();
                RunTransitions();
                break;

            case PlayerState.Jump:
                JumpActions();
                JumpTransitions();
                break;
        }
    }

    // Most physics related function in Fixed Update
    private void FixedUpdate()
    {
        // when it's in Run State, move rigid body
        if (pstate == PlayerState.Run)
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
        }

        // Check whether the players is on ground
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer); 
    }

    #region Idle State Related Functions
    private void IdleActions()
    {
        
    }

    private void IdleTransitions()
    {
        if (inputControl.Gameplay.Movement.IsPressed())
        {
            pstate = PlayerState.Run;
        }
        else if (inputControl.Gameplay.Jump.triggered)
        {
            pstate = PlayerState.Jump;
        }
    }
    #endregion

    #region Run State Related Functions
    private void RunActions()
    {
        // Flip sprite based on moving direction
        FlipSprite();
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
    }

    private void RunTransitions()
    {
        if (!inputControl.Gameplay.Movement.IsPressed())
        {
            pstate = PlayerState.Idle;
        }
        else if (inputControl.Gameplay.Jump.triggered)
        {
            pstate = PlayerState.Jump;
        }
    }

    private void FlipSprite()
    {
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }
    #endregion

    #region Jump State Related Functions
    private void JumpActions()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void JumpTransitions()
    {
        pstate = PlayerState.Idle;
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
