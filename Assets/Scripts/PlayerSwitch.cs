using UnityEngine;
using Cinemachine;

public class PlayerSwitch : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string characterName;
        public PlayerMovement movementScript;
        public CinemachineVirtualCamera vCam;
        public bool isUnlocked; // 각 캐릭터의 해금 상태
    }

    [Header("전체 캐릭터 리스트")]
    public PlayerData[] players;

    //현재 조작 중인 캐릭터 데이터
    private PlayerData currentPlayer;
    //현재 캐릭터가 실제로 밟고 서 있는 전환소의 캐릭터 데이터
    private PlayerData targetStationPlayer = null;

    void Start()
    {
        if (players == null || players.Length == 0) return;

        //Toto(0번)는 처음부터 해금
        players[0].isUnlocked = true;
        currentPlayer = players[0];

        ApplyControl();
    }

    void Update()
    {
        //전환소 안에 있고, 'E'키를 눌렀을 때만 발동
        if (targetStationPlayer != null && Input.GetKeyDown(KeyCode.E))
        {
            HandleSwitchLogic();
        }
    }

    private void HandleSwitchLogic()
    {
        if (targetStationPlayer == currentPlayer) return;

        if (!targetStationPlayer.isUnlocked)
        {
            if (currentPlayer.characterName == "Toto")
            {
                targetStationPlayer.isUnlocked = true;
                Debug.Log($"🎉 [Toto]가 [{targetStationPlayer.characterName}]을(를) 깨웠습니다! 이제 E키를 한 번 더 누르면 바꿀 수 있습니다.");
                return;
            }
            else
            {
                Debug.LogWarning($"⚠️ [{targetStationPlayer.characterName}]은(는) 오직 Toto만 처음 깨울 수 있습니다!");
                return;
            }
        }

        //캐릭터 스왑 진행
        currentPlayer = targetStationPlayer;
        ApplyControl();

        // 스왑된 직후에는 타겟 유효성을 순간적으로 비워줌, OnTriggerStay2D가 새 캐릭터 기준으로 안전하게 타겟을 다시 잡도록 유도
        targetStationPlayer = null;

        Debug.Log($"주도권 변경: 이제 [{currentPlayer.characterName}]을(를) 조작합니다.");
    }

    private void ApplyControl()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == currentPlayer)
            {
                // 현재 조작 중인 주인공 캐릭터는 무조건 1등 (Priority: 20)
                players[i].vCam.Priority = 20;
                players[i].movementScript.CameraController = true;
            }
            else
            {
                //조작권이 없는 나머지 캐릭터들은 이동을 끄고 카메라 우선순위를 바닥으로 내림
                players[i].movementScript.CameraController = false;

                // Toto는 기본 캐릭터이므로 조작권이 없을 때 Priority를 10으로, 나머지 서브 캐릭터들은 5로 줘서 서브 캐릭터들끼리 카메라가 겹치지 않게 함
                if (players[i].characterName == "Toto")
                {
                    players[i].vCam.Priority = 10;
                }
                else
                {
                    players[i].vCam.Priority = 5;
                }
            }
        }
    }

    //전환소 스크립트가 진입할 때 자기 주인의 스크립트를 던져주면, 매니저가 매칭
    public void SetTargetStation(bool isInside, PlayerMovement stationOwner)
    {
        if (isInside && stationOwner != null)
        {
            //던져준 스크립트와 일치하는 플레이어 데이터를 찾아서 타겟으로 설정
            foreach (var p in players)
            {
                if (p.movementScript == stationOwner)
                {
                    targetStationPlayer = p;
                    return;
                }
            }
        }
        else
        {
            //전환소에서 나가면 타겟을 비움
            targetStationPlayer = null;
        }
    }
}