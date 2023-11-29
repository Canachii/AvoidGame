using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public float speed = 2f;

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (player.position.y < -7.4)
            player.position = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (transform.position.y < 0)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }
}
