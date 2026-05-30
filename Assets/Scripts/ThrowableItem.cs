using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 💡 부모(Bubu)가 아이템을 던질 때 초기 속도를 주입해주는 함수
    public void Launch(Vector2 velocity)
    {
        if (rb != null)
        {
            rb.velocity = velocity;
        }
    }

    // 무언가와 부딪혔을 때의 처리 (예: 아이템 획득, 폭발 등)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 짚어둔 타겟이나 땅에 닿으면 사라지거나 효과 발동
        Debug.Log($"{collision.name}에 아이템이 도달했습니다!");
        Destroy(gameObject, 0.1f); // 우선은 부딪히면 사라지게 설정
    }
}