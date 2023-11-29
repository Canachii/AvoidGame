using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Glowing : MonoBehaviour
{
    [Range(0f, .04f)]
    public float speed = .02f;

    PostProcessVolume volume;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if (volume.weight <= 0)
        {
            volume.weight = 1;
        }
        else
            volume.weight -= speed;
    }
}
