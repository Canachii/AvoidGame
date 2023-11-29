using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    public GameObject projectile;
    public GameObject spike;
    public AudioClip[] sound;

    Animator anim;
    Transform target;
    CameraShake cam;
    AudioSource snd;

    int phaseNum;
    bool engage = true;
    float posX;
    float posY;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        snd = GetComponent<AudioSource>();
        target = GameObject.Find("Player").transform;
        posX = transform.position.x;
        posY = transform.position.y;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();

        StartCoroutine(StartBoss());
    }

    void FixedUpdate()
    {
        anim.SetInteger("Phase", phaseNum);
        switch (phaseNum)
        {
            case 1:
                InComing();
                break;

                case 2:
                Sliding();
                break;

            case 3:
                Spin();
                break;

            case 4:
                Walk();
                break;
        }
    }

    private void Walk()
    {
        if (transform.position.x > posX)
            transform.Translate(Vector2.left * .9f * Time.deltaTime);
    }

    private void Sliding()
    {
        if (transform.position.x > posX)
            transform.Translate(Vector2.left * 10f * Time.deltaTime);
    }

    void shot(int a)
    {
        //360번 반복
        for (int i = 0; i < 360; i += 13)
        {
            //총알 생성
            GameObject temp = Instantiate(projectile);

            Destroy(temp, 30f);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i + a * 20);
        }
    }

    IEnumerator Phase1()
    {
        phaseNum = 1;
        transform.localScale = new Vector3(1f, 1f, 1f);
        engage = true;

        transform.position = new Vector2(target.transform.position.x, transform.position.y + 18f);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 4; i++)
        {
            shot(i);
            yield return new WaitForSeconds(1f);
        }

        engage = false;
        yield return new WaitForSeconds(2f);
    }

    void InComing()
    {
        if (engage)
        {
            if (transform.position.y > posY)
            {
                anim.SetBool("isMoving", true);
                transform.Translate(Vector2.down * 30f * Time.deltaTime);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            anim.SetBool("isMoving", true);
            transform.Translate(Vector2.up * 30f * Time.deltaTime);
        }
    }

    IEnumerator Phase2()
    {
        phaseNum = 2;
        yield return new WaitForSeconds(4f);
        transform.localScale = new Vector3(-1f, 1f, 1f);
        transform.position = new Vector2(posX + 40f , posY);
        yield return new WaitForSeconds(4f);
    }

    IEnumerator Phase3()
    {
        phaseNum = 3;
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = new Vector2(0, 10f);
        for (int i = 0; i < 160; i++)
        {
            //총알 생성
            GameObject temp = Instantiate(projectile);

            Destroy(temp, 30f);

            temp.transform.position = transform.position;

            temp.transform.rotation = transform.rotation;

            yield return new WaitForSeconds(.02f);
        }
        yield return new WaitForSeconds(4f);
    }

    void Spin()
    {
        transform.Translate(Vector2.down * 3.5f * Time.deltaTime, Space.World);
        transform.Rotate(0, 0, -800f * Time.deltaTime);
    }

    IEnumerator Phase4()
    {
        phaseNum = 4;
        anim.SetBool("isMoving", false);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localScale = new Vector3(-5f, 5f, 5f);
        transform.position = new Vector2(20f, -2.5f);

        for (int i = 0; i < 280; i++)
        {
            float rnd = Random.Range(-15f, 15f);

            //총알 생성
            GameObject temp = Instantiate(spike);

            Destroy(temp, 30f);

            temp.transform.position = new Vector2(rnd, 10f);

            temp.transform.rotation = Quaternion.AngleAxis(Random.Range(-1f, 1f) * 20f, Vector3.forward);

            if (Random.Range(0, 10) > 4)
                cam.VibrateForTime(.1f);

            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    IEnumerator StartBoss()
    {
        yield return StartCoroutine(Phase1());
        yield return StartCoroutine(Phase2());
        yield return StartCoroutine(Phase3());
        yield return StartCoroutine(Phase4());
        yield return null;
    }
}
