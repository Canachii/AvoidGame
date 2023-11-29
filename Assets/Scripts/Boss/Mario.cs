using System.Collections;
using UnityEngine;

public class Mario : MonoBehaviour
{
    public GameObject projectile;
    public GameObject luigi;
    public AudioClip[] voice;
    public AudioClip[] sfx;

    public float power = 10f;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sp;
    AudioSource snd;

    Transform target;
    Vector3 def;
    CameraShake cam;
    ProjectileSpawner ps;

    int phaseNum;
    float posY;
    bool isJumping = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        snd = GetComponent<AudioSource>();

        target = GameObject.Find("Player").transform;
        posY = transform.position.y;
        def = transform.localScale;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        ps = GetComponent<ProjectileSpawner>();

        StartCoroutine(StartBoss());
    }

    void FixedUpdate()
    {
        switch (phaseNum)
        {
            case 1:
                Run();
                break;

                case 2:
                Fly();
                break;

            case 3:
                Jump();
                break;

            case 4:
                Bounce();
                break;
        }
    }

    private void Jump()
    {
        if (isJumping)
        {
            transform.Rotate(0, 0, 1f);
            rb.gravityScale = 2.5f;
        } else if (!isJumping)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }
    }

    private void Run()
    {
        if (transform.position.y > posY && isJumping)
        {
            transform.Translate(Vector2.down * 30f * Time.deltaTime);
        }
        else if (!isJumping && transform.position.x < target.position.x)
        {
            rb.AddForce(Vector2.right * 20f * Time.deltaTime, ForceMode2D.Impulse);
            sp.flipX = false;
        }
        else if (!isJumping && transform.position.x > target.position.x)
        {
            rb.AddForce(Vector2.left * 20f * Time.deltaTime, ForceMode2D.Impulse);
            sp.flipX = true;
        }

        if (!isJumping && Vector2.Distance(transform.position, target.position) <= 2.7f)
        {
            cam.VibrateForTime(.1f);
        }
    }

    void Fly()
    {
        if (isJumping)
        {
            transform.Translate(Vector2.up * 30f * Time.deltaTime);
        }

        if (transform.position.y < posY)
        {
            rb.velocity = Vector2.up * Random.Range(10f, 25f);
            cam.VibrateForTime(.1f);
        }
    }

    IEnumerator StartBoss()
    {
        yield return StartCoroutine(Phase1());
        yield return StartCoroutine(Phase2());
        yield return StartCoroutine(Phase3());
        yield return StartCoroutine(Phase4());
        yield return null;
    }

    IEnumerator Phase1()
    {
        snd.PlayOneShot(voice[0]);
        snd.PlayOneShot(sfx[0]);

        phaseNum = 1;
        anim.SetBool("isJumping", true);
        isJumping = true;
        transform.localScale = def;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        transform.position = new Vector2(target.transform.position.x, 15f);
        
        yield return new WaitForSeconds(.7f);
        cam.VibrateForTime(.1f);
        yield return new WaitForSeconds(.3f);

        anim.SetBool("isJumping", false);
        isJumping = false;

        yield return new WaitForSeconds(7f);
    }

    IEnumerator Phase2()
    {
        phaseNum = 2;
        anim.SetBool("isJumping", true);
        snd.PlayOneShot(voice[5]);
        isJumping = true;
        transform.localScale = def;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(3f);

        transform.position = new Vector2(14f, 10f);
        rb.gravityScale = 2.5f;
        anim.SetBool("fireMario", true);
        sp.flipX = true;
        isJumping = false;

        for (int i = 0; i < 22; i++)
        {
            yield return new WaitForSeconds(.3f);
            ps.TargetShot(projectile);
            snd.PlayOneShot(sfx[1]);
        }

        GameObject temp = Instantiate(luigi);
        snd.PlayOneShot(voice[4]);

        Destroy(temp, 19f);

        temp.transform.position = new Vector2(16, -7.5f);

        for (int i = 0; i < 22; i++)
        {
            yield return new WaitForSeconds(.3f);
            ps.TargetShot(projectile);
            snd.PlayOneShot(sfx[1]);
        }

        yield return null;
    }

    IEnumerator Phase3()
    {
        phaseNum = 3;
        anim.SetBool("isJumping", true);
        anim.SetBool("fireMario", true);
        isJumping = true;
        transform.localScale = def;

        yield return new WaitForSeconds(2f);

        transform.localScale = new Vector3(6f, 6f, 6f);

        for (int i = 0; i < 3; i++)
        {
            rb.angularVelocity = 0;
            transform.position = new Vector2(target.position.x, -10.6f);
            transform.rotation = Quaternion.Euler(0f, 0f, -45f);

            isJumping = false;

            snd.PlayOneShot(voice[1]);
            snd.PlayOneShot(sfx[3]);

            yield return new WaitForSeconds(3f);

            isJumping = true;
            rb.AddForce(Vector2.up * 24f, ForceMode2D.Impulse);
            cam.VibrateForTime(.3f);

            snd.PlayOneShot(voice[2]);
            snd.PlayOneShot(sfx[2]);

            for (int j = 0; j < 5; j++)
            {
                yield return new WaitForSeconds(.3f);

                ps.CircleShot(projectile, 13);
            }

            yield return new WaitForSeconds(4f);
        }
        yield return null;
    }

    IEnumerator Phase4()
    {
        phaseNum = 4;
        anim.SetBool("isJumping", true);
        anim.SetBool("fireMario", false);
        isJumping = true;
        transform.localScale = new Vector3(6f, 6f, 6f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.velocity = Vector2.zero;

        transform.position = new Vector2(-20f, 0f);
        snd.PlayOneShot(voice[3]);

        rb.gravityScale = 2.5f;
        rb.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(6f);
        isJumping = false;

        yield return null;
    }

    void Bounce()
    {
        if (isJumping)
        {
            if (rb.velocity.x > 0)
            {
                sp.flipX = false;
            }
            else if (rb.velocity.x < 0)
            {
                sp.flipX = true;
            }

            if (transform.position.y < -5f && rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                cam.VibrateForTime(.1f);
            }
            if (transform.position.x > 13.5f && rb.velocity.x > 0)
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            else if (transform.position.x < -13.5f && rb.velocity.x < 0)
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
    }
}
