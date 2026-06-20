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
                //바로 재시작하지 않고, 사망 프로세스 코루틴을 실행
                StartCoroutine(PlayerDieRoutine(collision.gameObject));
            }
        }
    }

    //사망 애니메이션을 기다린 후 씬을 재시작하는 코루틴
    private IEnumerator PlayerDieRoutine(GameObject playerBody)
    {
        //최상위 부모 오브젝트(PlayerMovement가 붙은 오브젝트)를 찾습니다.
        //자식 히트박스(Player_Body)에 맞았을 때를 대비해 부모 컴포넌트를 탐색합니다.
        PlayerMovement movement = playerBody.GetComponentInParent<PlayerMovement>();
        Animator animator = playerBody.GetComponentInChildren<Animator>();

        if (movement != null)
        {
            //플레이어 조작권을 즉시 압수하여 멈추게 만듭니다.
            movement.CameraController = false;
        }

        if (animator != null)
        {
            //사망 트리거 발동
            animator.SetTrigger("Die");

            //유니티가 애니메이터 상태를 'Dead'로 전환할 때까지 아주 잠깐(1프레임) 대기
            yield return null;

            //Dead 애니메이션의 실제 재생 시간(1초)만큼 대기합니다.
            Debug.Log($"사망 애니메이션 재생 시간: {1}초 대기 시작");
            yield return new WaitForSeconds(1);
        }
        else
        {
            //혹시라도 애니메이터를 못 찾아도 1초 대기
            yield return new WaitForSeconds(1.0f);
        }

        //애니메이션이 다 끝난 시점, 씬 재로딩
        Debug.Log("사망 애니메이션 완료 - 씬 재시작");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}