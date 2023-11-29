using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;

    int currentHealth;
    public static float currentScore;
    float score;

    private void Start()
    {
        currentScore = 0;
        score = -1;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = GameObject.Find("Player").GetComponent<PlayerController>().health;

        currentScore = Mathf.Round(score += Time.deltaTime);

        healthText.text = "HP : " + currentHealth.ToString() + " / 5";

        if (currentHealth > 0)
            scoreText.text = "Score : " + currentScore;

        if (currentHealth < 2)
        {
            healthText.color = Color.red;
        }
        else if (currentHealth < 3)
            healthText.color = Color.yellow;
    }
}
