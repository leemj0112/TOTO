using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public bool openBool = false;
    public Sprite openDoor;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (openBool && spriteRenderer.sprite != openDoor)
        {
            spriteRenderer.sprite = openDoor;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!openBool)
        {
            return;
        }

        if (collision.name == "Toto" || collision.name == "Toto Variant")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}