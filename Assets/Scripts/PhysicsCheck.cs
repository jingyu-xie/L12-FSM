using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [SerializeField]
    private float checkRadius;
    public LayerMask groundLayer;
    private bool isGrounded;

    private void Update()
    {
        Check();
    }

    private void Check()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundLayer);
    }
}
