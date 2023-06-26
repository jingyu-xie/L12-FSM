using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSaw : MonoBehaviour
{
    // Design Variables
    [SerializeField]
    private float speed;

    // Component Variables
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject leftPoint, rightPoint;
    private bool isMovingRight;
    public Vector2 currentTarget;

    private Vector2 knockBackDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = rightPoint.transform.localPosition;
        isMovingRight = true;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (transform.position.x > collision.gameObject.transform.position.x)
                knockBackDirection = new Vector2(-7, 10);
            else    
                knockBackDirection = new Vector2(7, 10);

            collision.rigidbody.AddForce(knockBackDirection, ForceMode2D.Impulse);
        }
    }
}
