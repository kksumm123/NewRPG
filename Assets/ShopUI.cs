using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
{
    CanvasGroup canvasGroup;
    GameObject shopMenu;
    GameObject subCategory;
    Text npcTalkBoxText;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        shopMenu = transform.Find("ShopMenu").gameObject;
        subCategory = transform.Find("SubCategory").gameObject;
        npcTalkBoxText = transform.Find("NPCTalkBox/Text").GetComponent<Text>();
        npcTalkBoxText.text = "";
        transform.Find("CloseButton/Button").GetComponent<Button>().onClick
                                            .AddListener(() => CloseUI());
    }

    private void OnEnable()
    {
        StageManager.GameState = GameStateType.Menu;
    }
    private void OnDisable()
    {
        //if (Application. == true)
            //return;
        StageManager.GameState = GameStateType.Play;
    }

    internal void ShowUI()
    {
        shopMenu.SetActive(true);
        subCategory.SetActive(false);
        gameObject.SetActive(true);

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);

        NPCTalkBoxText("무엇을 구매할거야?");
    }

    float speechSpeed = 20f;
    private void NPCTalkBoxText(string showText)
    {
        npcTalkBoxText.text = "";
        npcTalkBoxText.DOKill();
        npcTalkBoxText.DOText(showText, showText.Length / speechSpeed)
                      .SetUpdate(true);
    }

    void CloseUI()
    {
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true)
                   .OnComplete(() => gameObject.SetActive(false));
    }
}
