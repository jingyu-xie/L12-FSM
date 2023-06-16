using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    private enum PlayerState { Idle, Run, Jump}

    private Animator playerAnimator;
    private PlayerState pstate;

    private void Start()
    {
        pstate = PlayerState.Idle;
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
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

    private void IdleActions()
    {
        playerAnimator.SetTrigger("idle");
    }

    private void IdleTransitions()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("start jump");
            pstate = PlayerState.Jump;
        }
    }

    private void RunActions()
    {
        playerAnimator.SetTrigger("running");
    }

    private void RunTransitions()
    {
        
    }

    private void JumpActions()
    {
        playerAnimator.SetTrigger("jump");
    }

    private void JumpTransitions()
    {
        
    }
}
