using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSaw : MonoBehaviour
{
    // Design Variables
    [SerializeField]
    private float speed;

    // Component Variables
    private CircleCollider2D sawCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        sawCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < 1f)
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
        else
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
    }
}
