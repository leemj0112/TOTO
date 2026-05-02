using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [Header("시네머신 설정")]
    public CinemachineVirtualCamera targetCam; //Player(1) 전용 카메라

    [Header("스크립트 연결")]
    public PlayerMovement originalPlayer;  //Player
    public PlayerMovement secondaryPlayer; //Player(1)

    private bool isPlayerInside = false; //플레이어가 전환 범위 안에 있는지 여부

    void Start()
    {
        //1. Player(1) 카메라의 우선순위 조정
        targetCam.Priority = 5;

        //조작권 초기화
        originalPlayer.CameraController = true;
        secondaryPlayer.CameraController = false;

        Debug.Log("게임 시작: 기본 플레이어로 조작권을 설정했습니다.");
    }

    void Update()
    {
        //범위 안에서 R키 클릭 시 주도권 전환
        if (isPlayerInside && Input.GetKeyDown(KeyCode.R))
        {
            ToggleControl();
        }
    }

    private void ToggleControl()
    {
        //targetCam의 우선순위가 낮다면 -> Player(1)로 주도권 넘기기
        if (targetCam.Priority < 20)
        {
            targetCam.Priority = 20;

            //조작권 스왑
            originalPlayer.CameraController = false;
            secondaryPlayer.CameraController = true;

            Debug.Log("주도권 변경: 이제 Player(1)을 조작합니다.");
        }
        //이미 주도권을 가진 상태라면 원래 플레이어로 복구
        else
        {
            targetCam.Priority = 5;

            //조작권 스왑
            originalPlayer.CameraController = true;
            secondaryPlayer.CameraController = false;

            Debug.Log("주도권 복구: 다시 원래 캐릭터를 조작합니다.");
        }
    }

    //Player 태그를 가진 물체가 들어오면 조작 가능 상태로 변경
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("전환 범위 진입");
        }
    }

    //범위를 벗어나면 조작 불가능 상태로 변경
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("전환 범위 이탈");
        }
    }
}