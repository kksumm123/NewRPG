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
    public InventoryItemInfo inventoryItemInfo;
    public void LinkComponent()
    {
        button = GetComponent<Button>();
        icon = transform.Find("Icon").GetComponent<Image>();
        count = transform.Find("Count").GetComponent<Text>();
        activeGo = transform.Find("Selected")?.gameObject;
    }

    internal void Init(InventoryItemInfo item)
    {
        inventoryItemInfo = item;
        if (item != null)
        {
            icon.enabled = true;
            if (item.type == QuickSlotType.Item)
            {
                icon.sprite = item.ItemInfo.Sprite;
                icon.SetNativeSize();
                icon.transform.localScale = Vector3.one * 2.3f;
                count.text = item.count.ToString();
            }
            else if (item.type == QuickSlotType.Skill)
            {
                icon.sprite = item.SkillInfo.Sprite;
                icon.SetNativeSize();
                icon.transform.localScale = Vector3.one * 0.5f;
                count.text = string.Empty;
            }
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            count.text = "";
        }
    }
}
