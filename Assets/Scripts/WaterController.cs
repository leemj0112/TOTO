using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterController : MonoBehaviour
{
    public GameObject Morang;
    public GameObject pad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Body"))
        {
            Debug.Log("물: 플레이어와 부딪힘");

            if (collision.name == "Morang")
            {
                if (pad != null) pad.SetActive(true);
                return;
            }
            else
            {
                // [수정] 바로 재시작하지 않고, 사망 프로세스 코루틴을 실행합니다.
                StartCoroutine(PlayerDieRoutine(collision.gameObject));
            }
        }
    }

    // 사망 애니메이션을 기다린 후 씬을 재시작하는 코루틴
    private IEnumerator PlayerDieRoutine(GameObject playerBody)
    {
        // 1. 최상위 부모 오브젝트(PlayerMovement가 붙은 오브젝트)를 찾습니다.
        // 자식 히트박스(Player_Body)에 맞았을 때를 대비해 부모 컴포넌트를 탐색합니다.
        PlayerMovement movement = playerBody.GetComponentInParent<PlayerMovement>();
        Animator animator = playerBody.GetComponentInChildren<Animator>();

        if (movement != null)
        {
            // 플레이어 조작권을 즉시 압수하여 멈추게 만듭니다.
            movement.CameraController = false;
        }

        if (animator != null)
        {
            // 2. 사망 트리거 발동!
            animator.SetTrigger("Die");

            // 유니티가 애니메이터 상태를 'Dead'로 전환할 때까지 아주 잠깐(1프레임) 기다려줍니다.
            yield return null;

            // 3. 현재 재생 중인 애니메이션(Dead)의 정보를 가져옵니다.
            // SkeletonMecanim이 기본적으로 0번 레이어를 사용하므로 0을 넣어줍니다.
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 4. Dead 애니메이션의 실제 재생 시간(초)만큼 대기합니다.
            Debug.Log($"사망 애니메이션 재생 시간: {stateInfo.length}초 대기 시작");
            yield return new WaitForSeconds(stateInfo.length);
        }
        else
        {
            // 혹시라도 애니메이터를 못 찾으면 에러 방지를 위해 1초만 대기
            yield return new WaitForSeconds(1.0f);
        }

        // 5. 애니메이션이 다 끝난 시점이므로 안전하게 씬 재로딩!
        Debug.Log("사망 애니메이션 완료 - 씬 재시작");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}