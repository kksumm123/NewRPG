using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ShopUI : Singleton<ShopUI>
{
    Text selectedItemTypeTitle;
    ShopItemBaseBox baseShopItemBaseBox;
    Button npcTalkBoxOKButton;
    Text shopItemType;

    void InitBuyAndSellUI()
    {
        selectedItemTypeTitle = transform.Find("SubCategory/TypeDetail/Title/Title/TextParent/Text").GetComponent<Text>();
        shopItemType = transform.Find("SubCategory/BuyAndSell/ShopItemType/Text").GetComponent<Text>();
        baseShopItemBaseBox = transform.Find("SubCategory/TypeDetail/Scroll View/Viewport/ItemList/ItemBaseBox").GetComponent<ShopItemBaseBox>();
        baseShopItemBaseBox.LinkComponent();
        npcTalkBoxOKButton = transform.Find("NPCTalkBox/OKButton").GetComponent<Button>();
        itemTypeBaseBox = transform.Find("SubCategory/BuyAndSell/ShopItemType/ItemTypeList/BaseBox")
                                   .GetComponent<TextButtonBox>();
    }
    TextButtonBox itemTypeBaseBox;
    List<GameObject> itemTypeBaseBoxs = new List<GameObject>();
    List<GameObject> shopItemBaseBoxs = new List<GameObject>();

    void ShowBuyUI()
    {
        shopItemType.text = "Buy";
        ShowBuyAndSellUI(ShowBuyList);
        ShowBuyList(ItemType.Weapon);
    }
    void ShowSellUI()
    {
        shopItemType.text = "Sell";
        ShowBuyAndSellUI(ShowSellList);
        ShowSellList(ItemType.Weapon);
    }
    void ShowBuyAndSellUI(Action<ItemType> action)
    {
        SwitchShopMenuAndSubCategory();

        InitCategory();

        void InitCategory()
        {
            List<Tuple<string, UnityAction>> cmdList = new List<Tuple<string, UnityAction>>();
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Weapon), () => action(ItemType.Weapon)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Armor), () => action(ItemType.Armor)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Accessory), () => action(ItemType.Accessory)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Consume), () => action(ItemType.Consume)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Material), () => action(ItemType.Material)));

            itemTypeBaseBoxs.ForEach(x => Destroy(x));
            itemTypeBaseBoxs.Clear();

            itemTypeBaseBox.LinkComponent();
            itemTypeBaseBox.gameObject.SetActive(true);
            foreach (var item in cmdList)
            {
                var newButton = Instantiate(itemTypeBaseBox, itemTypeBaseBox.transform.parent);
                newButton.text.text = item.Item1;
                newButton.button.onClick.AddListener(item.Item2);
                itemTypeBaseBoxs.Add(newButton.gameObject);
            }
            ShowBuyList(ItemType.Weapon);
            itemTypeBaseBox.gameObject.SetActive(false);
        }

    }
    void ShowBuyList(ItemType itemType)
    {
        selectedItemTypeTitle.text = GetItemTypeString(itemType);

        shopItemBaseBoxs.ForEach(x => Destroy(x));
        shopItemBaseBoxs.Clear();

        // 리스트 표시
        List<ItemInfo> showItemList = ItemDB.Instance.GetItems(itemType);
        baseShopItemBaseBox.gameObject.SetActive(true);
        foreach (var item in showItemList)
        {
            var newItemBox = Instantiate(baseShopItemBaseBox, baseShopItemBaseBox.transform.parent);
            newItemBox.Init(item);
            newItemBox.button.onClick.AddListener(() => OnClick(item));
            shopItemBaseBoxs.Add(newItemBox.gameObject);
        }
        baseShopItemBaseBox.gameObject.SetActive(false);
        void OnClick(ItemInfo item)
        {
            print(item.name);
            SetNPCTalkBoxText($"{item.name}, 이거 구매할래?"
                , () =>
                {
                    print($"{item.name} 구매 확인 클릭");
                    // 유저에게 아이템 데이터 넘겨주자
                    var newItem = new InventoryItemInfo();
                    string result = UserData.Instance.ProcessBuy(item, 1);
                    SetNPCTalkBoxText(result);
                    ShowBuyList(itemType);
                });
        }
    }
    void ShowSellList(ItemType itemType)
    {
        selectedItemTypeTitle.text = GetItemTypeString(itemType);

        shopItemBaseBoxs.ForEach(x => Destroy(x));
        shopItemBaseBoxs.Clear();
        // 리스트 표시
        List<InventoryItemInfo> showItemList = UserData.Instance.GetItems(itemType);

        baseShopItemBaseBox.gameObject.SetActive(true);
        foreach (var item in showItemList)
        {
            var newItemBox = Instantiate(baseShopItemBaseBox, baseShopItemBaseBox.transform.parent);
            newItemBox.Init(item.ItemInfo);
            newItemBox.button.onClick.AddListener(() => OnClick(item));
            shopItemBaseBoxs.Add(newItemBox.gameObject);
        }
        baseShopItemBaseBox.gameObject.SetActive(false);

        void OnClick(InventoryItemInfo item)
        {
            string itemName = item.ItemInfo.name;
            print(itemName);
            SetNPCTalkBoxText($"{itemName}, 이거 판매할래?"
                , () =>
                {
                    print($"{itemName} 판매 확인 클릭");
                    // 유저에게 아이템 데이터 넘겨주자
                    var newItem = new InventoryItemInfo();
                    string result = UserData.Instance.ProcessSell(item, 1);
                    SetNPCTalkBoxText(result);
                    ShowSellList(itemType);
                });
        }
    }

    string GetItemTypeString(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Money:
                return "재화";
            case ItemType.Weapon:
                return "무기";
            case ItemType.Armor:
                return "방어구";
            case ItemType.Accessory:
                return "악세서리";
            case ItemType.Consume:
                return "소비";
            case ItemType.Material:
                return "재료";
            case ItemType.Etc:
                return "기타";
            default:
                return "";
        }
    }
}
