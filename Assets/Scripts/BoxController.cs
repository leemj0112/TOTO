using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoxController : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y <= -7.5f) //만약 Box가 -7.5f 이상 떨어질 시
        {
            Destroy(gameObject); //오브젝트 삭제, 이후 스포너가 재생성
        }
    }
}
