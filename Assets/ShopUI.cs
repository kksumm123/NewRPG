using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopUI : Singleton<ShopUI>
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
    TextButtonBox buyBaseBox;
    void ShowBuyUI()
    {
        SwitchShopMenuAndSubCategory();

        buyBaseBox = transform.Find("SubCategory/Buy/ItemType/ItemTypeList/BaseBox")
                           .GetComponent<TextButtonBox>();
        InitCategory();

        void InitCategory()
        {
            List<Tuple<string, UnityAction>> cmdList = new List<Tuple<string, UnityAction>>();
            cmdList.Add(new Tuple<string, UnityAction>("무기", () => ShowBuyList(ItemType.Weapon)));
            cmdList.Add(new Tuple<string, UnityAction>("방어구", () => ShowBuyList(ItemType.Armor)));
            cmdList.Add(new Tuple<string, UnityAction>("악세서리", () => ShowBuyList(ItemType.Accessory)));
            cmdList.Add(new Tuple<string, UnityAction>("소비아이템", () => ShowBuyList(ItemType.Consume)));
            cmdList.Add(new Tuple<string, UnityAction>("재료", () => ShowBuyList(ItemType.Material)));

            buyBaseBox.LinkComponent();

            buyBaseBox.gameObject.SetActive(true);
            foreach (var item in cmdList)
            {
                var newButton = Instantiate(buyBaseBox, buyBaseBox.transform.parent);
                newButton.text.text = item.Item1;
                newButton.button.onClick.AddListener(item.Item2);
            }
            buyBaseBox.gameObject.SetActive(false);

        }
    }
    void ShowBuyList(ItemType itemType)
    {
        print(itemType.ToString());
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

        NPCTalkBoxText("안녕, 반가워. 무엇을 할래?");
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
