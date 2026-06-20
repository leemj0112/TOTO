using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public GameObject itemPrefab;      
    public Transform throwPoint;        
    public LineRenderer lineRenderer;   

    [Header("발사 세팅")]
    public float throwPower = 15f;     
    public int trajectoryResolution = 30; 

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        //라인 렌더러 기본 세팅 코드 자동화
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = trajectoryResolution;
            lineRenderer.enabled = false; //처음엔 안 보이게 설정
        }
    }

    void Update()
    {
        if (playerMovement == null || !playerMovement.CameraController)
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            return;
        }

        //마우스 오른쪽 버튼을 누르고 있는 동안 조준 및 궤적 표시
        if (Input.GetMouseButton(1))
        {
            lineRenderer.enabled = true;
            Vector2 launchVelocity = CalculateLaunchVelocity();
            DrawTrajectory(launchVelocity);

            //조준 중 마우스 왼쪽 버튼을 누르면 발사!
            if (Input.GetMouseButtonDown(0))
            {
                LaunchItem(launchVelocity);
            }
        }

        //마우스 우클릭을 떼면 궤적 숨기기
        if (Input.GetMouseButtonUp(1))
        {
            lineRenderer.enabled = false;
        }
    }

    //마우스 위치를 기반으로 발사 속도(방향 + 힘) 계산
    Vector2 CalculateLaunchVelocity()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        //조준점(마우스)과 발사 위치 사이의 방향 벡터 구하기
        Vector2 direction = (mousePos - throwPoint.position).normalized;

        //최종 속도 = 방향 * 힘
        return direction * throwPower;
    }

    //물리 법칙에 따른 미래의 위치(포물선)를 계산해서 선으로 그리기
    void DrawTrajectory(Vector2 startVelocity)
    {
        Vector2 startPosition = throwPoint.position;
        float g = Mathf.Abs(Physics2D.gravity.y); //유니티 월드의 중력 값 값 가져오기

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float time = i * 0.05f;

            float x = startPosition.x + (startVelocity.x * time);
            float y = startPosition.y + (startVelocity.y * time) - (0.5f * g * time * time);

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    //아이템 생성 및 발사
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