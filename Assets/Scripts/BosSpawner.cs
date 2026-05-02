using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BosSpawner : MonoBehaviour
{
    public GameObject boxPrefab;    //소환할 박스 프리팹
    private GameObject currentBox;  //현재 이 스포너가 관리하는 박스

    void Start()
    {
        SpawnBox();
    }

    void Update()
    {
        //자신이 소환한 박스가 파괴되면 즉시 재생성
        if (currentBox == null)
        {
            SpawnBox();
        }
    }

    void SpawnBox()
    {
        if (boxPrefab != null)
        {
            //스포너의 현재 위치 박스 생성
            currentBox = Instantiate(boxPrefab, transform.position, transform.rotation);
        }
    }
}
