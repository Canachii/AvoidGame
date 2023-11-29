using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 10f;

    public bool moveToRight = true;
    public bool moveToDown = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!moveToDown && moveToRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
        else if (!moveToDown && !moveToRight)
            transform.Translate(Vector2.left * speed * Time.deltaTime, Space.Self);
        else if (moveToDown && moveToRight)
            transform.Translate(Vector2.down * speed * Time.deltaTime, Space.Self);
        else if (moveToDown && !moveToRight)
            transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
    }
}
