using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    ItemBox baseItemBox;
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
    void Awake()
    {
        transform.Find("CloseButton/Button").GetComponent<Button>().onClick
                                            .AddListener(() => CloseUI());

        List<TextButtonBox> categoryBox = new List<TextButtonBox>();
        for (int i = 1; i <= 6; i++)
        {
            var button = transform.Find($"Inventory/Category/CategoryBox{i}").GetComponent<TextButtonBox>();
            button.LinkComponent();
            categoryBox.Add(button);
            var tmp_i = i;
            categoryBox[tmp_i - 1].button.onClick.AddListener(() => ShowItemCategory((ItemType)tmp_i));
        }
        baseItemBox = transform.Find("Inventory/TypeDetail/Scroll View/Viewport/Content/ItemBox").GetComponent<ItemBox>();
        baseItemBox.LinkComponent();
    }

    List<GameObject> itemboxs = new List<GameObject>();
    void ShowItemCategory(ItemType itemType)
    {
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
            ItemInfoUI.Instance.ShowUI(item);
        }
    }

    public void ShowUI()
    {
        ShowItemCategory(ItemType.Weapon);
    }
    void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
