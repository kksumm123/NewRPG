using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemBaseBox : MonoBehaviour
{
    [SerializeField] Text itemName;
    [SerializeField] Text price;
    [SerializeField] Image icon;

    public void LinkComponent()
    {
        itemName = transform.Find("ItemName").GetComponent<Text>();
        price = transform.Find("Price").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();

    }
    internal void Init(ItemInfo item)
    {
        price.text = item.buyPrice.ToString();
        itemName.text = item.name;
        icon.sprite = item.Sprite;
    }
}
