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
        for (int i = 0; i < 6; i++)
        {
            categoryBox.Add(transform.Find($"Inventory/Category/CategoryBox{i}").GetComponent<TextButtonBox>());
            categoryBox[i].button.onClick.AddListener(() => ShowItemCategory((ItemType)i));
        }

        baseItemBox = transform.Find("Inventory/TypeDetail/Scroll View/Viewport/Content/Item").GetComponent<ItemBox>();
    }

    List<GameObject> itemboxs = new List<GameObject>();
    void ShowItemCategory(ItemType itemType)
    {
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
}
