using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    ItemBox baseItemBox;
    void Awake()
    {
        List<TextButtonBox> categoryBox = new List<TextButtonBox>();
        for (int i = 1; i <= 6; i++)
        {
            var button = transform.Find($"Inventory/Category/CategoryBox{i}").GetComponent<TextButtonBox>();
            button.LinkComponent();
            categoryBox.Add(button);
            categoryBox[i - 1].button.onClick.AddListener(() => ShowItemCategory((ItemType)i));
            print($"{(ItemType)i} 연결해따");
        }
        //categoryBox[0].button.onClick.AddListener(() => ShowItemCategory(ItemType.Weapon));
        //categoryBox[1].button.onClick.AddListener(() => ShowItemCategory(ItemType.Armor));
        //categoryBox[2].button.onClick.AddListener(() => ShowItemCategory(ItemType.Accessory));
        //categoryBox[3].button.onClick.AddListener(() => ShowItemCategory(ItemType.Consume));
        //categoryBox[4].button.onClick.AddListener(() => ShowItemCategory(ItemType.Material));
        //categoryBox[5].button.onClick.AddListener(() => ShowItemCategory(ItemType.Etc));
        baseItemBox = transform.Find("Inventory/TypeDetail/Scroll View/Viewport/Content/ItemBox").GetComponent<ItemBox>();
        baseItemBox.LinkComponent();
    }

    List<GameObject> itemboxs = new List<GameObject>();
    void ShowItemCategory(ItemType itemType)
    {
        print($"{itemType} 클릭해따");

        gameObject.SetActive(true);
        itemboxs.ForEach(x => Destroy(x));
        itemboxs.Clear();

        // 리스트 표시
        List<InventoryItemInfo> showItemList = UserData.Instance.GetItems(itemType);

        baseItemBox.gameObject.SetActive(true);
        foreach (var item in showItemList)
        {
            var newItemBox = Instantiate(baseItemBox, baseItemBox.transform.parent);
            newItemBox.button.onClick.AddListener(() => OnClick(item));
            newItemBox.Init(item);
            itemboxs.Add(newItemBox.gameObject);
        }
        baseItemBox.gameObject.SetActive(false);
        void OnClick(InventoryItemInfo item)
        {
            string itemName = item.ItemInfo.name;
            print(itemName);
        }
    }
    public void ShowUI()
    {
        ShowItemCategory(ItemType.Weapon);
    }
}
