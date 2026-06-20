using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    private float horizontal;
    private float speed = 8f;   //이동 속도
    private float JumpingPower = 32f;   //점프 파워
    private bool isFacingRIght = true;  //게임 캐릭터가 오른쪽 보고 있는지 확인

    public bool CameraController = true;

    [SerializeField] private Rigidbody2D rb; 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private new BoxCollider2D collider2D;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Animator animator;

    private void Start()
    {
        // Start 안의 GetComponentInChildren 코드는 과감히 지우거나 주석 처리합니다.
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

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
                if (animator != null) animator.SetTrigger("doJump");
            }
            else
            {
                // 점프 버튼을 누르지 않은 평소에는 doJump 트리거가 남아있지 않도록 강제로 리셋합니다.
                if (animator != null && IsGrounded())
                {
                    animator.ResetTrigger("doJump");
                }
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

        // [애니메이션 상태 실시간 업데이트]
        if (animator != null)
        {
            // 1. 걷기/대기 제어: 좌우 입력 크기를 넘겨줍니다 (0이면 Idle, 0보다 크면 Walk)
            animator.SetFloat("MoveSpeed", Mathf.Abs(horizontal));

            // 2. 땅 위에 있는지 여부를 전달합니다.
            animator.SetBool("isGrounded", IsGrounded());

            // 3. Y축 속도를 전달하여 상승(Loop)/하강(End)을 구분합니다.
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
    }

    //velocity에 이동값 들어가는 코드
    private void FixedUpdate()
    {
        if (CameraController)
        {
            //조작 중일 때는 좌우 이동 가능, 회전 방지
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            //조작권이 없을 때: 
            //속도를 즉시 0으로 만듦
            horizontal = 0f;
            rb.velocity = Vector2.zero;

            //위치를 고정
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        if (transform.position.y <= -7.5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    //임시 스테이지 1 이동 코드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Door_Stage_1")
        {
            SceneManager.LoadScene("Stage1Scene");
        }
    }
}
