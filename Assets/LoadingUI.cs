using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : BaseUI<LoadingUI>
{
    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.root);
    }
    public void ShowLoadingUI(AsyncOperation result)
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowUICo(result));
    }

    Text percent;
    Image progressBar;
    private IEnumerator ShowUICo(AsyncOperation result)
    {
        percent = transform.Find("Text").GetComponent<Text>();
        progressBar = transform.Find("ProgressBar").GetComponent<Image>();

        while (result.isDone == false)
        {
            percent.text = (result.progress * 100).ToString() + " %";
            progressBar.fillAmount = result.progress;
            yield return null;
        }
        percent.text = "100 %";
        progressBar.fillAmount = 1;
        CloseUI();
    }
}
