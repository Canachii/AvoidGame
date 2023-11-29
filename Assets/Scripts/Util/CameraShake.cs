using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = .1f;

    float shakeTime;
    Vector3 defPos;

    public void VibrateForTime(float time)
    {
        shakeTime = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + defPos;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = defPos;
        }
    }
}
