using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildActivator : MonoBehaviour
{
    private void Awake()
    {
        // 자식 오브젝트 검색, 활성화
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
