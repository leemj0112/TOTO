using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    private PlayerSwitch manager;
    private PlayerMovement myOwnerMovement;

    void Start()
    {
        manager = FindObjectOfType<PlayerSwitch>();
        myOwnerMovement = GetComponentInParent<PlayerMovement>();
    }

    // 💡 [핵심 추가] 트리거 안에 머무르는 동안 실시간으로 체크
    private void OnTriggerStay2D(Collider2D other)
    {
        if (manager == null || myOwnerMovement == null) return;

        if (other.CompareTag("Player_Body"))
        {
            var walkerMovement = other.GetComponentInParent<PlayerMovement>();

            // 🌟 캐릭터 조작권이 방금 막 나에게 넘어왔다면, 
            // 가만히 서 있어도 실시간으로 매니저에게 이 전환소를 타겟으로 잡으라고 갱신해줌!
            if (walkerMovement != null && walkerMovement.CameraController)
            {
                manager.SetTargetStation(true, myOwnerMovement);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (manager == null || myOwnerMovement == null) return;

        if (other.CompareTag("Player_Body"))
        {
            var walkerMovement = other.GetComponentInParent<PlayerMovement>();
            if (walkerMovement != null && walkerMovement.CameraController)
            {
                manager.SetTargetStation(true, myOwnerMovement);
                Debug.Log($"[{myOwnerMovement.gameObject.name}]의 전환소 진입 (밟은 사람: {walkerMovement.gameObject.name})");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (manager == null || myOwnerMovement == null) return;

        if (other.CompareTag("Player_Body"))
        {
            var walkerMovement = other.GetComponentInParent<PlayerMovement>();

            // 내가 조작하던 캐릭터가 이 범위를 완전히 벗어날 때만 타겟을 해제
            if (walkerMovement != null && walkerMovement.CameraController)
            {
                manager.SetTargetStation(false, null);
                Debug.Log($"[{myOwnerMovement.gameObject.name}]의 전환소 이탈");
            }
        }
    }
}