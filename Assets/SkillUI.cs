using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseUI<T> : Singleton<T> where T : MonoBehaviour
{
    CanvasGroup canvasGroup;
    void OnEnable()
    {
        StageManager.GameState = GameStateType.Menu;
    }
    void OnDisable()
    {
        //if (Application. == true)
        //return;
        StageManager.GameState = GameStateType.Play;
    }
    public void ShowUI()
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);
    }
    public void CloseUI()
    {
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true)
                   .OnComplete(() => gameObject.SetActive(false));
    }
}
public class SkillUI : BaseUI<SkillUI>
{
    
}
