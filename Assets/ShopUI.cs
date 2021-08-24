using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ShopUI : Singleton<ShopUI>
{
    CanvasGroup canvasGroup;
    GameObject shopMenu;
    GameObject subCategory;
    Text npcTalkBoxText;

    TextButtonBox categoryBaseBox;
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
        InitBuyUI();

        // Buy, Sell, Craft, Exit
        categoryBaseBox = transform.Find("ShopMenu/Category/BaseBox")
                           .GetComponent<TextButtonBox>();
        InitCategory();

        void InitCategory()
        {
            // tuple 구조를 안쓰면
            //List<TempClases> cmdList = new List<TempClases>();
            //cmdList.Add(new TempClases() { str = "Buy", fn = ShowBuyUI });

            List<Tuple<string, UnityAction>> cmdList = new List<Tuple<string, UnityAction>>();
            cmdList.Add(new Tuple<string, UnityAction>("Buy", ShowBuyUI));
            cmdList.Add(new Tuple<string, UnityAction>("Sell", ShowSellUI));
            cmdList.Add(new Tuple<string, UnityAction>("Craft", ShowCraftUI));
            cmdList.Add(new Tuple<string, UnityAction>("Exit", CloseUI));

            categoryBaseBox.LinkComponent();

            categoryBaseBox.gameObject.SetActive(true);
            foreach (var item in cmdList)
            {
                var newButton = Instantiate(categoryBaseBox, categoryBaseBox.transform.parent);
                newButton.text.text = item.Item1;
                newButton.button.onClick.AddListener(item.Item2);
            }
            categoryBaseBox.gameObject.SetActive(false);
        }
    }

    void SwitchShopMenuAndSubCategory()
    {
        shopMenu.SetActive(false);
        subCategory.SetActive(true);
    }

    void ShowSellUI()
    {
        throw new NotImplementedException();
    }

    void ShowCraftUI()
    {
        throw new NotImplementedException();
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

        SetNPCTalkBoxText("안녕, 반가워. 무엇을 할래?");
    }

    float speechSpeed = 20f;
    private void SetNPCTalkBoxText(string showText, Action action = null)
    {
        npcTalkBoxText.text = "";
        npcTalkBoxText.DOKill();
        npcTalkBoxText.DOText(showText, showText.Length / speechSpeed)
                      .SetUpdate(true);

        if (action == null)
            npcTalkBoxOKButton.gameObject.SetActive(false);
        else
        {
            npcTalkBoxOKButton.gameObject.SetActive(true);
            npcTalkBoxOKButton.onClick.RemoveAllListeners();
            npcTalkBoxOKButton.onClick.AddListener(() => { action(); });
        }
    }

    void CloseUI()
    {
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true)
                   .OnComplete(() => gameObject.SetActive(false));
    }
}
