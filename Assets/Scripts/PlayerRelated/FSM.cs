using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FSM : MonoBehaviour
{
    #region All Variables
    // State Related Variables
    private enum PlayerState { Idle, Run, FastRun, Jump, DoubleJump}
    private PlayerState pstate;

    // Component Variables
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
        Debug.Log(pstate.ToString());
        // Read the direction of movement from input
        inputDirection = inputControl.Gameplay.Movement.ReadValue<Vector2>();

        // Function to switch animation
        SetAnimation();

        // Flip sprite based on moving direction
        FlipSprite();

        // Check whether the players is on ground
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);

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

            case PlayerState.FastRun:
                FastRunActions();
                FastRunTransitions();
                break;

            case PlayerState.Jump:
                JumpActions();
                JumpTransitions();
                break;

            case PlayerState.DoubleJump:
                DoubleJumpActions();
                DoubleJumpTransitions();
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

        if (pstate == PlayerState.FastRun)
        {
            rb.velocity = new Vector2(inputDirection.x * 2f * speed * Time.deltaTime, rb.velocity.y);
        }
    }

    #region Idle State
    private void IdleActions()
    {

    }

    private void IdleTransitions()
    {
        if (inputControl.Gameplay.Movement.IsPressed())
        {
            if (inputControl.Gameplay.FastRun.IsPressed())
                pstate = PlayerState.FastRun;
            else
                pstate = PlayerState.Run;
        }

        if (inputControl.Gameplay.Jump.triggered && isGrounded)
        {
            pstate = PlayerState.Jump;
        }
    }
    #endregion

    #region Run State
    private void RunActions()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
    }

    private void RunTransitions()
    {
        if (!inputControl.Gameplay.Movement.IsPressed())
        {
            pstate = PlayerState.Idle;
        }
        
        if (inputControl.Gameplay.FastRun.IsPressed())
        {
            pstate = PlayerState.FastRun;
        }

        if (inputControl.Gameplay.Jump.triggered && isGrounded)
        {
            pstate = PlayerState.Jump;
        }
    }
    #endregion

    #region Fast Run State
    private void FastRunActions()
    {
        rb.velocity = new Vector2(inputDirection.x * 2f * speed * Time.deltaTime, rb.velocity.y);
    }

    private void FastRunTransitions()
    {
        if (!inputControl.Gameplay.Movement.IsPressed())
        {
            pstate = PlayerState.Idle;
        }

        if (!inputControl.Gameplay.FastRun.IsPressed())
            pstate = PlayerState.Run;

        if (inputControl.Gameplay.Jump.triggered && isGrounded)
        {
            pstate = PlayerState.Jump;
        }
    }
    #endregion

    #region Jump State
    private void JumpActions()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void JumpTransitions()
    {
        if (isGrounded)
            pstate = PlayerState.Idle;
        else if (inputControl.Gameplay.Jump.triggered)
            pstate = PlayerState.DoubleJump;
    }
    #endregion

    private void DoubleJumpActions()
    {
        Debug.Log("double jump");
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); 
    }

    private void DoubleJumpTransitions()
    {
        pstate = PlayerState.Idle;
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

    private void FlipSprite()
    {
        if (inputDirection.x > 0)
            spriteRenderer.flipX = false;
        if (inputDirection.x < 0)
            spriteRenderer.flipX = true;
    }
}
