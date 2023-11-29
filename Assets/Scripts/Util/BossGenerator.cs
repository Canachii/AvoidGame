using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public GameObject[] prefabs;
    public AudioClip[] bgm;

    public float lifeTime = 60f;
    public float delay = 2f;

    [Header("BGM")]
    public float maxVolume = .5f;
    public float fadeOut = 5f;

    AudioSource snd;

    int num;
    int pre = -1;
    bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        snd = GetComponent<AudioSource>();
        snd.volume = 0;

        Time.timeScale = 1;

        StartCoroutine(RandomNum());
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (snd.volume < maxVolume)
                snd.volume += maxVolume / fadeOut * Time.deltaTime;
        }
        else
        {
            snd.volume -= maxVolume / fadeOut * Time.deltaTime;
        }
    }

    IEnumerator RandomNum()
    {
        num = Random.Range(0, prefabs.Length);
        if (num == pre)
        {
            yield return StartCoroutine(RandomNum());
        }
        else
        {
            pre = num;
            Debug.Log("Current Boss : " + prefabs[num]);
            snd.clip = bgm[num];
            yield return StartCoroutine(SpawnBoss());
        }
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(delay);

        snd.Play();
        isPlaying = true;

        GameObject temp = Instantiate(prefabs[num]);
        Destroy(temp, lifeTime);

        yield return new WaitForSeconds(lifeTime - fadeOut);

        isPlaying = false;

        yield return new WaitForSeconds(fadeOut);

        snd.Stop();

        yield return StartCoroutine(RandomNum());
    }
}
