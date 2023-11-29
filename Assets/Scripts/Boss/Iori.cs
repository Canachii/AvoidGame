using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iori : MonoBehaviour
{
    public GameObject[] attack;
    public AudioClip[] voice;
    public float speed = 20f;

    Animator anim;
    SpriteRenderer sr;
    AudioSource snd;

    Transform target;
    Vector2 pos;
    CameraShake cam;

    int phase;
    bool isMoving;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        snd = GetComponent<AudioSource>();

        target = GameObject.Find("Player").transform;
        pos = transform.position;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

        phase = 0;
        isMoving = false;

        StartCoroutine(StartBoss());
    }

    void Update()
    {
        switch (phase)
        {
            case 1:
                MoveDown();
                break;

            case 2:
                MoveUp();
                break;

            case 3:
                MoveDown();
                break;

            case 4:
                moveSide();
                break;
        }
    }

    private void moveSide()
    {
        if (isMoving)
        {
            sr.flipX = true;
            transform.Translate(Vector2.right * speed * .4f * Time.deltaTime);
        }
        else
        {
            sr.flipX = false;
            transform.Translate(Vector2.left * speed * .4f * Time.deltaTime);
        }
    }

    private void MoveUp()
    {
        if (isMoving)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void MoveDown()
    {
        if (transform.position.y > pos.y)
            transform.Translate(Vector2.down * speed * Time.deltaTime);
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
        transform.position = new Vector2(13, 10);
        phase = 1;

        anim.SetBool("Jump", true);

        yield return new WaitForSeconds(.9f);

        anim.SetBool("Jump", false);

        yield return new WaitForSeconds(2f);

        anim.SetBool("useUlt", true);

        yield return new WaitForSeconds(.2f);
        snd.PlayOneShot(voice[0]);
        yield return new WaitForSeconds(.8f);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 3; j > -7; j -= 2)
            {
                GameObject temp = Instantiate(attack[0]);

                Destroy(temp, 2f);

                if (i > 0)
                {
                    temp.transform.position = new Vector2(j * 3, -9f);
                }
                else
                {
                    temp.transform.position = new Vector2((j + 1f) * 3, -9f);
                }

                cam.VibrateForTime(.2f);

                yield return new WaitForSeconds(.3f);
            }
        }

        anim.SetBool("useUlt", false);

        yield return new WaitForSeconds(1f);

        anim.SetBool("useSide", true);
        snd.PlayOneShot(voice[2]);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 3; i++)
        {
            GameObject temp = Instantiate(attack[1]);

            Destroy(temp, 2f);

            if (i % 2 == 0)
            {
                temp.transform.position = new Vector2(-7.5f, -9f);
            }
            else
            {
                temp.transform.position = new Vector2(7.5f, -9f);
            }

            cam.VibrateForTime(.3f);

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    IEnumerator Phase2()
    {
        phase = 2;
        anim.SetBool("useSide", false);

        yield return new WaitForSeconds(.5f);

        isMoving = true;
        anim.SetBool("Jump", true);

        yield return new WaitForSeconds(2f);
        snd.PlayOneShot(voice[1]);

        isMoving = false;
        anim.SetBool("Jump", false);

        cam.VibrateForTime(.1f);

        for (int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(attack[2]);

            Destroy(temp, 5f);

            temp.transform.position = new Vector2(-20, -8.3f);

            GameObject tmp = Instantiate(attack[2]);

            Destroy(tmp, 5f);

            tmp.transform.position = new Vector2(20, -8.3f);
            tmp.GetComponent<ProjectileController>().moveToRight = false;

            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < 2; i++)
        {
            for (int j = 3; j > -7; j -= 2)
            {
                GameObject temp = Instantiate(attack[0]);

                Destroy(temp, 2f);

                if (i > 0)
                {
                    temp.transform.position = new Vector2(j * 3, -9f);
                }
                else
                {
                    temp.transform.position = new Vector2((j + 1f) * 3, -9f);
                }

                cam.VibrateForTime(.2f);

                yield return new WaitForSeconds(.3f);
            }
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 4; i++)
        {
            GameObject temp = Instantiate(attack[1]);

            Destroy(temp, 2f);

            if (i % 2 == 0)
            {
                temp.transform.position = new Vector2(-7.5f, -9f);
            }
            else
            {
                temp.transform.position = new Vector2(7.5f, -9f);
            }

            cam.VibrateForTime(.3f);

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    IEnumerator Phase3()
    {
        transform.position = new Vector2(-14, 10);
        phase = 3;
        sr.flipX = true;

        anim.SetBool("Jump", true);

        yield return new WaitForSeconds(.9f);

        anim.SetBool("Jump", false);

        yield return new WaitForSeconds(2f);

        anim.SetBool("useSide", true);
        snd.PlayOneShot(voice[3]);

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 60; i++)
        {
            if (i % Random.Range(3, 5) == 0)
            {
                GameObject temp = Instantiate(attack[3]);

                Destroy(temp, 5f);

                temp.transform.position = new Vector2(transform.position.x, -5);

                yield return new WaitForSeconds(.2f);
            }
            else
            {
                GameObject temp = Instantiate(attack[2]);

                Destroy(temp, 5f);

                temp.transform.position = transform.position;

                cam.VibrateForTime(.1f);

                yield return new WaitForSeconds(.2f);
            }
        }

        anim.SetBool("useSide", false);

        yield return new WaitForSeconds(1f);

        yield return null;
    }

    IEnumerator Phase4()
    {
        phase = 4;

        anim.SetBool("JumpA", true);
        snd.PlayOneShot(voice[2]);

        for (int i = 0; i < 7; i++)
        {
            if (i % 2 == 0)
            {
                GameObject temp = Instantiate(attack[2]);

                Destroy(temp, 10f);

                temp.transform.position = transform.position;

                cam.VibrateForTime(.1f);
                
                isMoving = true;
                yield return new WaitForSeconds(Random.Range(2, 3));
            }
            else
            {
                GameObject temp = Instantiate(attack[2]);

                Destroy(temp, 10f);

                temp.transform.position = transform.position;
                temp.GetComponent<ProjectileController>().moveToRight = false;

                cam.VibrateForTime(.1f);

                isMoving = false;
                yield return new WaitForSeconds(Random.Range(2, 3));
            }
        }

        yield return null;
    }
}
