using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Toto")
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("문은 토토만 열 수 있습니다!");
        }
    }
}
