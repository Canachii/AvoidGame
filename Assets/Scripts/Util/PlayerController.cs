using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public AudioClip jumpSound1;
    public AudioClip jumpSound2;
    public AudioClip hitSound;
    public float moveSpeed = 6f;
    public float jumpPower = 6f;
    public float jumpTime = 0.3f;
    public int maxJumpCount = 1;
    public float godTime = 3f;

    Rigidbody2D rb;
    BoxCollider2D coll;
    SpriteRenderer sp;
    Animator anim;
    AudioSource snd;

    int jumpCount;
    float moveX;
    bool isJumping = false;
    float jumpTimer;
    bool godMode = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        snd = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            jumpCount = maxJumpCount;
        }

        moveX = Input.GetAxisRaw("Horizontal");

        if (moveX == 0)
        {
            anim.SetBool("isMoving", false);
        }
        else if (moveX > 0)
        {
            sp.flipX = false;
            anim.SetBool("isMoving", true);
        }
        else if (moveX < 0)
        {
            sp.flipX = true;
            anim.SetBool("isMoving", true);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount > 0)
            {
                isJumping = true;
                jumpTimer = 0;
                anim.SetBool("isJumping", true);
                if (IsGrounded())
                {
                    jumpCount--;
                    snd.PlayOneShot(jumpSound1);
                }
                else
                {
                    jumpCount--;
                    snd.PlayOneShot(jumpSound2);
                }
            }
        }

        if (isJumping)
            jumpTimer += Time.deltaTime;

        if (Input.GetButtonUp("Jump") | jumpTimer > jumpTime)
        {
            isJumping = false;
            anim.SetBool("isJumping", false);
        }

        if (IsGrounded() || isJumping)
            anim.SetBool("isFalling", false);
        else if (!IsGrounded() && !isJumping)
            anim.SetBool("isFalling", true);

        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
            jumpCount = maxJumpCount;

        Move();
        Jump();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * moveX, rb.velocity.y);
    }

    private void Jump()
    {
        if (!isJumping)
            return;

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, LayerMask.GetMask("Ground"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!godMode)
        {
            if (health > 1)
            {
                health--;
                snd.PlayOneShot(hitSound);
                StartCoroutine(HitTimer());
            }
            else if (health <= 1)
            {
                health = 0;
                Debug.Log("Game Over");
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    IEnumerator HitTimer()
    {
        godMode = true;
        sp.color = new Color(1f, 1f, 1f, .6f);
        yield return new WaitForSeconds(godTime);
        godMode = false;
        sp.color = new Color(1f, 1f, 1f, 1f);
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.transform.tag == "Ground")
    //    {
    //        jumpCount--;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground" && collision.contacts[0].normal.y <= 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
    }
}
