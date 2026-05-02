using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterController : MonoBehaviour
{
    public GameObject Morang;
    public GameObject pad;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Body"))
        {
            if (collision.name == "PLAYER (1)")
            {
                pad.SetActive(true);
                return;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
