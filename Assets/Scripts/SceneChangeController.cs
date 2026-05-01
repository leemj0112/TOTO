using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeController : MonoBehaviour
{
    //Game Scene으로 이동
    public void GoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    //Exit
    public void GeExit()
    {
        Application.Quit();
    }
}
