using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ShopUI : BaseUI<ShopUI>
{
    GameObject shopMenu;
    GameObject subCategory;
    Text npcTalkBoxText;

    TextButtonBox categoryBaseBox;
    new void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 0;
        shopMenu = transform.Find("ShopMenu").gameObject;
        subCategory = transform.Find("SubCategory").gameObject;
        npcTalkBoxText = transform.Find("NPCTalkBox/Text").GetComponent<Text>();
        npcTalkBoxText.text = "";
        transform.Find("CloseButton/Button").GetComponent<Button>().onClick
                                            .AddListener(() => CloseUI());
        InitBuyAndSellUI();

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
        shopMenu.GetComponent<CanvasGroup>().alpha = 1;
        shopMenu.GetComponent<CanvasGroup>().DOFade(0, 0.5f)
                            .SetUpdate(true)
                            .OnComplete(() => shopMenu.SetActive(false))
                            .SetLink(gameObject);
        subCategory.SetActive(true);
        subCategory.GetComponent<CanvasGroup>().alpha = 0;
        subCategory.GetComponent<CanvasGroup>().DOFade(1, 0.5f)
                            .SetUpdate(true)
                            .SetLink(gameObject);
    }

    void ShowCraftUI()
    {
        print("미구현");
    }

    public override void ShowUI()
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);

        shopMenu.GetComponent<CanvasGroup>().alpha = 1;
        shopMenu.SetActive(true);
        subCategory.SetActive(false);

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);

        SetNPCTalkBoxText("안녕, 반가워. 무엇을 할래?");
    }

    float speechSpeed = 20f;
    private void SetNPCTalkBoxText(string showText, Action action = null)
    {
        print(showText);
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
}
