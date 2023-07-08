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

    [SerializeField]
    private List<GameObject> pointList;
    private int currentTargetIndex;
    private bool isMovingTo;

    private Vector2 knockBackDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = rightPoint.transform.localPosition;
        isMovingRight = true;

        isMovingTo = true;
        currentTargetIndex = 1;
    }

    private void FixedUpdate()
    {
        //Movement();
    }

    private void Update()
    {
        //AnotherMovement();
        ListMovement();
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

    private void AnotherMovement()
    {
        if (Vector2.Distance(transform.localPosition, currentTarget) < 0.05f)
        {
            if (isMovingRight)
                currentTarget = leftPoint.transform.localPosition;
            else
                currentTarget = rightPoint.transform.localPosition;

            isMovingRight = !isMovingRight;
        }

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentTarget, speed * Time.deltaTime);
    }

    private void ListMovement()
    {
        //MoveTo(pointList[currentTargetIndex].transform.localPosition);
        MoveTo(pointList[currentTargetIndex].transform.localPosition);
    }

    private void MoveTo(Vector2 nextPosition)
    {
        if (Vector2.Distance(transform.localPosition, nextPosition) > 0.05f)
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, nextPosition, speed * Time.deltaTime);
        else
        {
            if (currentTargetIndex == pointList.Count - 1)
                isMovingTo = false;
            else if (currentTargetIndex == 0)
                isMovingTo = true;

            if (isMovingTo)
                MoveTo(pointList[++currentTargetIndex].transform.localPosition);
            else
                MoveTo(pointList[--currentTargetIndex].transform.localPosition);
        }
    }

    private void LoopMovement(Vector2 nextPosition)
    {
        if (Vector2.Distance(transform.localPosition, nextPosition) > 0.05f)
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, nextPosition, speed * Time.deltaTime);
        else
        {
            if (currentTargetIndex == pointList.Count - 1)
            {
                currentTargetIndex = 0;
            }
            else if (currentTargetIndex == 0)
            {
                currentTargetIndex++;
            }
            MoveTo(pointList[currentTargetIndex].transform.localPosition);
        }
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
