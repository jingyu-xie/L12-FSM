using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Animator : MonoBehaviour
{
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

    }
}
