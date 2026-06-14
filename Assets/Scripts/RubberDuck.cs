using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberDuck : MonoBehaviour
{
    public float jumpForce = 15f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 태그가 "Player"인 오브젝트와 충돌했을 때만 작동
        if (other.CompareTag("Player_Body"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            // 플레이어에게 Rigidbody가 있는지 확인
            if (rb != null)
            {
                Debug.Log("플레이어와 충돌함");
                // 플레이어의 Y축 방향으로 순간적인 힘(Impulse)을 가함
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 기존 Y속도 초기화
            }
            else
            {
                Debug.Log("플레이어 인식 못함");
            }
        }
    }
}