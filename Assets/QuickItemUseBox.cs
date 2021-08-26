using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ItemBox))]
public class QuickItemUseBox : MonoBehaviour, IDropHandler
{
    public ItemBox itembox;
    public Text number;
    public void LinkComponent()
    {
        number = transform.Find("Number").GetComponent<Text>();
        itembox = GetComponent<ItemBox>();
        itembox.LinkComponent();
    }
    public void OnDrop(PointerEventData eventData)
    {
        print(eventData);
        ItemBox fromItembox = eventData.pointerDrag.GetComponent<ItemBox>();
        itembox.Init(fromItembox.inventoryItemInfo);

        // 할당하면 UserData에 저장
        int itemUID = fromItembox.inventoryItemInfo.uid;
        UserData.Instance.itemData.data.quickItemUIDs[index] = itemUID;
    }

    int index;
    internal void Init(int i, InventoryItemInfo inventoryItemInfo, string keyBindingString)
    {
        index = i;
        itembox.Init(inventoryItemInfo);
        number.text = keyBindingString.Replace("<keyboard>/", "");

        GetComponent<ShortcutButton>().shortcutKey
            = new InputAction("key", InputActionType.Button, keyBindingString);
        GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    void OnClick()
    {
        print(number.text);
    }
}
