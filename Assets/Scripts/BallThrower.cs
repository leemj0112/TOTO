using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public GameObject itemPrefab;       // 1단계에서 만든 아이템 프리팹
    public Transform throwPoint;        // 아이템이 발사될 위치 (Bubu의 손 위치 오브젝트)
    public LineRenderer lineRenderer;   // 궤적을 그릴 라인 렌더러

    [Header("발사 세팅")]
    public float throwPower = 15f;      // 던지는 기본 힘의 세기
    public int trajectoryResolution = 30; // 궤적을 이룰 점의 개수 (많을수록 부드러움)

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // 라인 렌더러 기본 세팅 코드 자동화
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = trajectoryResolution;
            lineRenderer.enabled = false; // 처음엔 안 보이게 설정
        }
    }

    void Update()
    {
        // 🌟 Bubu가 현재 조작 중일 때만 작동하도록 방어벽 작동!
        if (playerMovement == null || !playerMovement.CameraController)
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            return;
        }

        // 1. 마우스 오른쪽 버튼을 누르고 있는 동안 조준 및 궤적 표시
        if (Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;
            Vector2 launchVelocity = CalculateLaunchVelocity();
            DrawTrajectory(launchVelocity);

            // 2. 조준 중 마우스 왼쪽 버튼을 누르면 발사!
            if (Input.GetMouseButtonDown(0))
            {
                LaunchItem(launchVelocity);
            }
        }

        // 마우스 우클릭을 떼면 궤적 숨기기
        if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
        }
    }

    // 🎯 마우스 위치를 기반으로 발사 속도(방향 + 힘) 계산
    Vector2 CalculateLaunchVelocity()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // 조준점(마우스)과 발사 위치 사이의 방향 벡터 구하기
        Vector2 direction = (mousePos - throwPoint.position).normalized;

        // 최종 속도 = 방향 * 힘
        return direction * throwPower;
    }

    // 📈 핵심: 물리 법칙에 따른 미래의 위치(포물선)를 계산해서 선으로 그리기
    void DrawTrajectory(Vector2 startVelocity)
    {
        Vector2 startPosition = throwPoint.position;
        float g = Mathf.Abs(Physics2D.gravity.y); // 유니티 월드의 중력 값 값 가져오기

        for (int i = 0; i < trajectoryResolution; i++)
        {
            // 각 점마다의 시간 계산 (매 프레임 미래의 시간)
            float time = i * 0.05f;

            // 포물선 공식: 등속 운동(X) + 중력 가속도 운동(Y)
            // $x = v_x \cdot t$
            // $y = v_y \cdot t - \frac{1}{2}g \cdot t^2$
            float x = startPosition.x + (startVelocity.x * time);
            float y = startPosition.y + (startVelocity.y * time) - (0.5f * g * time * time);

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // 🚀 실제 아이템 생성 및 발사
    void LaunchItem(Vector2 launchVelocity)
    {
        GameObject firedItem = Instantiate(itemPrefab, throwPoint.position, Quaternion.identity);
        ThrowableItem itemScript = firedItem.GetComponent<ThrowableItem>();

        if (itemScript != null)
        {
            itemScript.Launch(launchVelocity);
        }
    }
}