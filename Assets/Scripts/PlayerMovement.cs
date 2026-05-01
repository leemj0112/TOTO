using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    private float speed = 8f;
    private float JumpingPower = 32f;
    private bool isFacingRIght = true;

    public bool CameraController = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private new BoxCollider2D collider2D;

    void Update()
    {
        if (CameraController == true)
        {
            //Horizontal을 이용한 이동 코드
            horizontal = Input.GetAxis("Horizontal");

            //땅에 접촉 시에만 점프 조건 충족
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpingPower);
            }

            if (Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }

            //캐릭터 방향에 따라 Sprite 반전
            Flip();
        }
        else
        {
            return;
        }
    }

    //velocity에 이동값 들어가는 코드
    private void FixedUpdate()
    {
        if (CameraController)
        {
            // 조작 중일 때는 좌우 이동 가능, 회전만 방지
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            // 조작권이 없을 때: 
            // 1. 속도를 즉시 0으로 만듦
            horizontal = 0f;
            rb.velocity = Vector2.zero;

            // 2. 위치를 고정(FreezePosition)해서 외부 힘에 밀리지 않게 함
            // (단, 중력이 필요하다면 FreezePositionX만 사용해도 좋아)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    //groundLayer인지 확인
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //Sprite 반전 코드
    private void Flip()
    {
        if (isFacingRIght && horizontal < 0f || !isFacingRIght & horizontal > 0f)
        {
            isFacingRIght = !isFacingRIght;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Door_Stage_1")
        {
            SceneManager.LoadScene("Stage1Scene");
        }
    }
}
