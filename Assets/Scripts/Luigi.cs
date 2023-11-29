using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luigi : MonoBehaviour
{
    public float speed = 3f;

    SpriteRenderer sr;

    Vector2 pos1;
    Vector2 pos2;

    float delta;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        pos1 = new Vector2(-14.5f, -7.5f);
        pos2 = new Vector2(14.5f, -7.5f);

        delta = 0;
    }

    private void Update()
    {
        delta += Time.deltaTime;

        Vector2 v = new Vector2(0, -7.5f);

        if (Mathf.Sin(delta) > 0)
            sr.flipX = true;
        else if (Mathf.Sin(delta) < 0)
            sr.flipX = false;

        v.x += Vector2.Distance(pos1, pos2) * Mathf.Cos(delta * speed) / 2;
        transform.position = v;
    }
}
