using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapPortal : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 포탈 작동
            // 맵 로딩 비동기
            var result = SceneManager.LoadSceneAsync(sceneName);

            // 맵 로딩 UI
            // 프로그래스 바 표시
            LoadingUI.Instance.ShowLoadingUI(result);
        }
    }
}
