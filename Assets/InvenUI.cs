using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    void Awake()
    {
        List<TextButtonBox> categoryBox = new List<TextButtonBox>();
        for (int i = 0; i < 6; i++)
        {
            categoryBox.Add(transform.Find($"Inventory/Category/CategoryBox{i}").GetComponent<TextButtonBox>());
            categoryBox[i].button.onClick.AddListener(() => ShowItemCategory((ItemType)i));
        }
    }

    TextButtonBox baseItemBox;
    List<GameObject> itemboxs = new List<GameObject>();
    void ShowItemCategory(ItemType itemType)
    {
        itemboxs.ForEach(x => Destroy(x));
        itemboxs.Clear();

        // 리스트 표시
        List<ItemInfo> showItemList = ItemDB.Instance.GetItems(itemType);

        baseItemBox.gameObject.SetActive(true);
        foreach (var item in showItemList)
        {
            var newItemBox = Instantiate(baseItemBox, baseItemBox.transform.parent);
            newItemBox.button.onClick.AddListener(() => OnClick(item));
            itemboxs.Add(newItemBox.gameObject);
        }
        baseItemBox.gameObject.SetActive(false);
        void OnClick(ItemInfo item)
        {
            print(item.name);
        }
    }
}
