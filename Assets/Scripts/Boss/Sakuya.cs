using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sakuya : MonoBehaviour
{
    public GameObject[] projectile;
    public AudioClip[] sound;

    Animator anim;
    SpriteRenderer sr;
    AudioSource snd;

    CameraShake cam;
    ProjectileSpawner ps;

    Transform target;

    int phase;
    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        snd = GetComponent<AudioSource>();

        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        ps = GetComponent<ProjectileSpawner>();

        target = GameObject.Find("Player").transform;

        phase = 0;

        StartCoroutine(StartBoss());
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 1:
                Intro();
                break;

            case 2:
                break;
        }
    }

    private void Intro()
    {
        if (isMoving)
        {
            anim.SetBool("walk", true);
            transform.Translate(Vector2.right * 4 * Time.deltaTime);
        }
        else
            anim.SetBool("walk", false);
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
        transform.position = new Vector2(-17, -6.2f);
        phase = 1;

        isMoving = true;

        yield return new WaitForSeconds(1);

        isMoving = false;

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 4; i++)
        {
            anim.SetTrigger("teleport");
            snd.PlayOneShot(sound[3]);
            yield return new WaitForSeconds(.3f);
            transform.position = new Vector2(Random.Range(-13, 13), Random.Range(-4, 6));

            if (transform.position.x > target.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;

            yield return new WaitForSeconds(.5f);

            anim.SetTrigger("spellAa");
            yield return new WaitForSeconds(.5f);
            snd.PlayOneShot(sound[Random.Range(4, 6)]);
            ps.CircleTargetShot(projectile[1], 13, .5f);

            yield return new WaitForSeconds(1f);
        }

        anim.SetTrigger("teleport");
        snd.PlayOneShot(sound[3]);
        yield return new WaitForSeconds(.3f);
        transform.position = new Vector2(14, -6.2f);
        sr.flipX = true;

        yield return new WaitForSeconds(1f);

        for (int i = 10; i > 0; i--)
        {
            anim.SetTrigger("shotBa");

            yield return new WaitForSeconds(.5f);
            snd.PlayOneShot(sound[Random.Range(4, 6)]);

            int num = Random.Range(-8, -2);

            for (int j = -8; j < num; j++)
            {
                GameObject temp = Instantiate(projectile[0]);

                Destroy(temp, 5);

                temp.transform.position = new Vector2(transform.position.x, j);

                temp.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }

            for (int j = num + 1; j < -2; j++)
            {
                GameObject temp = Instantiate(projectile[0]);

                Destroy(temp, 5);

                temp.transform.position = new Vector2(transform.position.x, j);

                temp.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            }

            cam.VibrateForTime(.1f);

            yield return new WaitForSeconds(i / 10f);
        }

        yield return new WaitForSeconds(2f);

        yield return null;
    }

    IEnumerator Phase2()
    {
        phase = 2;

        anim.SetTrigger("ult");
        snd.PlayOneShot(sound[1]);

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 3; i++)
        {
            snd.PlayOneShot(sound[0]);
            Time.timeScale = 0;

            for (int j = 0; j < 100; j++)
            {
                GameObject temp = Instantiate(projectile[0]);

                Destroy(temp, 10);

                temp.transform.position = new Vector2(Random.Range(-13, 13), Random.Range(-6, 6));

                temp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));

                yield return new WaitForSecondsRealtime(.01f);
            }

            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 1;
            snd.PlayOneShot(sound[2]);
            cam.VibrateForTime(.2f);

            yield return new WaitForSeconds(2f);
        }

        yield return null;
    }

    IEnumerator Phase3()
    {
        phase = 3;

        anim.SetTrigger("teleport");
        snd.PlayOneShot(sound[1]);
        yield return new WaitForSeconds(.3f);
        transform.position = Vector2.up * 6;
        sr.flipX = false;

        yield return new WaitForSeconds(1f);

        anim.SetTrigger("spellAa");

        for (int i = 0; i < 13; i++)
        {
            yield return new WaitForSeconds(.5f);

            transform.position = new Vector2(Random.Range(-13, 13), 6);
            ps.CircleShot(projectile[1], 13);
            snd.PlayOneShot(sound[Random.Range(4, 5)]);

            StartCoroutine(TimeStop(.1f));
        }

        yield return StartCoroutine(TimeStop(.5f));

        anim.SetTrigger("spellAa");

        yield return new WaitForSeconds(.5f);

        snd.PlayOneShot(sound[6]);

        for (int i = 0; i < 850; i++)
        {
            yield return new WaitForSeconds(.01f);
            ps.SpinShot(projectile[1], 20);

            if (i % 20 == 0)
            {
                StartCoroutine(TimeStop(.1f));
            }
        }

        transform.rotation = Quaternion.Euler(Vector3.zero);

        yield return new WaitForSeconds(1f);

        anim.SetTrigger("teleport");
        snd.PlayOneShot(sound[3]);
        yield return new WaitForSeconds(.3f);

        transform.position = Vector2.up * 12;

        yield return null;
    }

    IEnumerator TimeStop(float time)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1;
        cam.VibrateForTime(.1f);
        yield return new WaitForSeconds(time);
    }
}
