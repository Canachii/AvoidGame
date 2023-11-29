using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public AudioClip sound;
    public float power = 100f;

    GameObject player;

    Animator anim;
    Rigidbody2D rb;
    AudioSource snd;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        snd = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < -7.5f && rb.velocity.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y / 1.5f);

        if (transform.position.x > 14.5f && rb.velocity.x > 0)
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        else if (transform.position.x < -14.5f && rb.velocity.x < 0)
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            anim.SetTrigger("Collision");
            snd.PlayOneShot(sound);
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * power, ForceMode2D.Impulse);
        }
    }
}
