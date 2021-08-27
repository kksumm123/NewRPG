using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : Singleton<ItemInfoUI>
{
    ItemBox itembox;
    Text title;
    Text description;

    void Start()
    {
        itembox = GetComponentInChildren<ItemBox>();
        itembox.LinkComponent();
        title = transform.Find("Title").GetComponent<Text>();
        description = transform.Find("Description").GetComponent<Text>();
    }

    public void ShowUI(InventoryItemInfo item)
    {
        title.text = item.ItemInfo.name;
        description.text = item.ItemInfo.description;
        itembox.Init(item);
    }
}
