using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemBox : MonoBehaviour
{
    public Button button;
    public Image icon;
    public Text count;
    public GameObject activeGo;
    public void LinkComponent()
    {
        button = GetComponent<Button>();
        icon = transform.Find("Icon").GetComponent<Image>();
        count = transform.Find("Count").GetComponent<Text>();
        activeGo = transform.Find("Selected").gameObject;
    }

    internal void Init(InventoryItemInfo item)
    {
        icon.sprite = item.ItemInfo.Sprite;
        count.text = item.count.ToString();
    }
}
