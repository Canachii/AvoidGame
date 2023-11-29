using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ryu : MonoBehaviour
{
    int phase;

    public GameObject[] projectile;
    public AudioClip[] voice;
    public AudioClip[] sfx;

    SpriteRenderer sr;
    Animator anim;
    AudioSource snd;

    CameraShake cam;
    ProjectileSpawner ps;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        snd = GetComponent<AudioSource>();

        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        ps = GetComponent<ProjectileSpawner>();

        phase = 0;

        StartCoroutine(StartBoss());
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;
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
        phase = 1;
        yield return new WaitForSeconds(2f);

        anim.SetTrigger("hadouken");
        yield return new WaitForSeconds(.2f);

        ps.NormalShot(projectile[1]);
        cam.VibrateForTime(.3f);
        yield return StartCoroutine(Phase1());
        yield return null;
    }

    IEnumerator Phase2()
    {
        phase = 2;
        yield return null;
    }

    IEnumerator Phase3()
    {
        phase = 3;
        yield return null;
    }
}
