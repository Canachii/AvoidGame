using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOnClick : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
