using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;
    void Start()
    {
        scoreTxt.text = "Score : " + UiController.currentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
