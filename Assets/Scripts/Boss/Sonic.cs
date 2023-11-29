using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonic : MonoBehaviour
{
    public GameObject spike;
    public GameObject spring;
    public GameObject ring;
    public GameObject stage;
    public AudioClip[] voice;
    public AudioClip[] se;

    public float speed = 10f;
    public float maxX = 30f;
    public float maxY = 30f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    TrailRenderer tr;
    AudioSource snd;
    CameraShake cam;

    int phaseN = 0;
    float moveX;
    float moveY;
    bool isRunning = false;
    Vector2 defPos;
    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();
        snd = GetComponent<AudioSource>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        tr.enabled = false;

        target = GameObject.Find("Player").transform;

        defPos = transform.position;

        StartCoroutine(StartBoss());
    }

    private void Update()
    {
        bool isJumping = false;

        moveX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetButtonUp("Jump") | moveY > .3f)
        {
            isJumping = false;
        }

        if (isJumping)
            moveY += Time.deltaTime;
        else if (!isJumping)
        {
            moveY = 0;
        }
    }

    void FixedUpdate()
    {
        switch (phaseN)
        {
            case 1:
                GoLeft();
                break;

            case 2:
                FlyToTarget();
                break;

            case 3:
                Fly();
                break;
        }
    }

    private void Fly()
    {
        rb.velocity = rb.velocity;

        if (transform.position.x > -20 && isRunning)
        {
            transform.Translate(Vector2.left * 100f * Time.deltaTime);
            sr.flipX = true;
        }
        else if (transform.position.x < 20 && !isRunning)
        {
            transform.Translate(Vector2.right * 100f * Time.deltaTime);
            sr.flipX = false;
        }
    }

    private void NewStage(float lifeTime)
    {
        GameObject ground = Instantiate(stage);

        Destroy(ground, lifeTime);

        ground.transform.position = Vector2.down * 5f;
    }

    private void FlyToTarget()
    {
        if (isRunning)
        {
            //벡터 뻴셈을 통해 방향을 구함
            var dir = (target.position - transform.position).normalized;

            var dist = Vector2.Distance(transform.position, target.position);

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

            //해당 방향으로 회전한다.
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            rb.AddForce(dir * dist * speed * Time.deltaTime, ForceMode2D.Impulse);

            rb.AddForce(moveX * Vector2.right * speed * Time.deltaTime, ForceMode2D.Impulse);
            rb.AddForce(moveY * Vector2.up * speed * Time.deltaTime, ForceMode2D.Impulse);

            if (rb.velocity.x > maxX)
                rb.velocity = new Vector2(maxX, rb.velocity.y);
            if (rb.velocity.x < -maxX)
                rb.velocity = new Vector2(-maxX, rb.velocity.y);
            if (rb.velocity.y > maxY)
                rb.velocity = new Vector2(rb.velocity.x, maxY);
            if (rb.velocity.x < -maxY)
                rb.velocity = new Vector2(rb.velocity.x, -maxY);
        }
    }

    private void GoLeft()
    {
        if (isRunning)
        {
            transform.Translate(Vector2.left * 100f * Time.deltaTime);
            sr.flipX = true;
        }
        else if (!isRunning)
        {
            transform.Translate(Vector2.right * 10f * Time.deltaTime);
            sr.flipX = false;
        }
    }

    IEnumerator StartBoss()
    {
        yield return StartCoroutine(Phase1());
        yield return StartCoroutine(Phase2());
        yield return StartCoroutine(Phase3());
        yield return null;
    }

    IEnumerator Phase1()
    {
        snd.PlayOneShot(voice[0]);

        for (int i = -15; i < 16; i++)
        {
            GameObject temp = Instantiate(spike);

            Destroy(temp, 60f);

            temp.transform.position = new Vector2(i, 8.5f);
        }

        isRunning = false;
        yield return new WaitForSeconds(2f);

        phaseN = 1;
        isRunning = true;
        transform.position = defPos;

        yield return new WaitForSeconds(1f);

        isRunning = false;
        transform.position = new Vector2(-20f, 7f);
        anim.SetBool("isBall", true);

        yield return new WaitForSeconds(.8f);

        for (int i = 0; i < 8; i++)
        {
            GameObject temp = Instantiate(spring);

            Destroy(temp, 7f);

            temp.transform.position = transform.position;

            temp.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-90f, 90f));

            Rigidbody2D rb = temp.GetComponent<Rigidbody2D>();

            rb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(.35f);
        }

        yield return new WaitForSeconds(5.7f);

        isRunning = true;

        for (int i = 0; i < 2; i++)
        {
            snd.PlayOneShot(se[0]);
            yield return new WaitForSeconds(.5f);
            transform.position = defPos + Vector2.down * .1f;
            yield return new WaitForSeconds(.5f);
        }

        yield return null;
    }

    IEnumerator Phase2()
    {
        phaseN = 2;
        anim.SetBool("isSuper", true);
        sr.flipX = false;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        transform.position = Vector2.up * 6;
        rb.gravityScale = 0;
        isRunning = false;
        tr.enabled = true;

        snd.PlayOneShot(voice[1]);

        cam.VibrateForTime(2f);
        NewStage(45f);

        yield return new WaitForSeconds(2f);

        isRunning = true;
        rb.AddForce(Vector2.one * speed * 50 * Time.deltaTime, ForceMode2D.Impulse);

        for (int i = 0; i < 28; i++)
        {
            GameObject temp = Instantiate(ring);

            Destroy(temp, 20f);

            temp.transform.position = transform.position;

            var dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            temp.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Random.Range(0, 10) > 2)
                cam.VibrateForTime(.5f);

            yield return new WaitForSeconds(1f);
        }

        isRunning = false;

        yield return new WaitForSeconds(2f);

        yield return null;
    }

    IEnumerator Phase3()
    {
        phaseN = 3;

        tr.enabled = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rb.velocity = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            if (i % 2 == 1)
            {
                snd.PlayOneShot(voice[2]);

                yield return new WaitForSeconds(1f);

                isRunning = false;
                transform.position = new Vector2(-20f, target.transform.position.y);
                tr.enabled = true;

                cam.VibrateForTime(.1f);

                yield return new WaitForSeconds(3f);
                tr.enabled = false;
            }
            else
            {
                snd.PlayOneShot(voice[2]);

                yield return new WaitForSeconds(1f);

                isRunning = true;
                transform.position = new Vector2(20f, target.transform.position.y);
                tr.enabled = true;

                cam.VibrateForTime(.1f);

                yield return new WaitForSeconds(3f);
                tr.enabled = false;
            }
        }

        yield return null;
    }
}
