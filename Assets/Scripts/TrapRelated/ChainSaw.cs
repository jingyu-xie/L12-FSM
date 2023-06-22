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
    [SerializeField]
    private GameObject leftPoint, rightPoint;
    private bool isMovingRight;
    public Vector2 currentTarget;

    private void Awake()
    {
        sawCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        currentTarget = rightPoint.transform.localPosition;
        isMovingRight = true;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.localPosition, currentTarget) < 0.05f)
        {
            if (isMovingRight)
                currentTarget = leftPoint.transform.localPosition;
            else
                currentTarget = rightPoint.transform.localPosition;

            isMovingRight = !isMovingRight;
        }

        if (isMovingRight)
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
        else 
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);


    }
}
