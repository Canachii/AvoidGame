using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounce : MonoBehaviour
{
    public bool onGround = true;
    public bool onWall = true;
    public float divide = 1.5f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (onGround)
        {
            if (transform.position.y < -8f && rb.velocity.y < 0)
                rb.velocity = new Vector2(rb.velocity.x / divide, -rb.velocity.y / divide);
        }

        if (onWall)
        {
            if (transform.position.x > 15f && rb.velocity.x > 0)
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            else if (transform.position.x < -15f && rb.velocity.x < 0)
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
    }
}
