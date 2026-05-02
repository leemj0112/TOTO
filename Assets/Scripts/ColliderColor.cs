using UnityEngine;

public class ColliderColor : MonoBehaviour
{
    public Color gizmoColor = new Color(1f, 0f, 0f, 0.5f); //반투명 빨간색

    void OnDrawGizmos()
    {
        BoxCollider2D box2d = GetComponent<BoxCollider2D>();
        if (box2d != null)
        {
            Gizmos.color = gizmoColor;
            // 콜라이더의 위치와 크기에 맞춰 큐브를 그림
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box2d.offset, box2d.size);
        }
    }
}
