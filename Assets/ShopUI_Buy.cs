using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class ShopUI : Singleton<ShopUI>
{
    Text selectedItemTypeTitle;

    void InitBuyTitle()
    {
        selectedItemTypeTitle = transform.Find("SubCategory/TypeDetail/Title/Title/TextParent/Text").GetComponent<Text>();
    }
    TextButtonBox buyBaseBox;
    List<GameObject> buyBaseBoxs = new List<GameObject>();
    void ShowBuyUI()
    {
        SwitchShopMenuAndSubCategory();

        buyBaseBox = transform.Find("SubCategory/Buy/ItemType/ItemTypeList/BaseBox")
                           .GetComponent<TextButtonBox>();
        InitCategory();

        void InitCategory()
        {
            List<Tuple<string, UnityAction>> cmdList = new List<Tuple<string, UnityAction>>();
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Weapon), () => ShowBuyList(ItemType.Weapon)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Armor), () => ShowBuyList(ItemType.Armor)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Accessory), () => ShowBuyList(ItemType.Accessory)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Consume), () => ShowBuyList(ItemType.Consume)));
            cmdList.Add(new Tuple<string, UnityAction>(GetItemTypeString(ItemType.Material), () => ShowBuyList(ItemType.Material)));

            buyBaseBoxs.ForEach(x => Destroy(x));
            buyBaseBoxs.Clear();

            buyBaseBox.LinkComponent();
            buyBaseBox.gameObject.SetActive(true);
            foreach (var item in cmdList)
            {
                var newButton = Instantiate(buyBaseBox, buyBaseBox.transform.parent);
                newButton.text.text = item.Item1;
                newButton.button.onClick.AddListener(item.Item2);
                buyBaseBoxs.Add(newButton.gameObject);
            }
            buyBaseBox.gameObject.SetActive(false);
        }
    }
    void ShowBuyList(ItemType itemType)
    {
        print(itemType.ToString());
        selectedItemTypeTitle.text = GetItemTypeString(itemType);
      
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
