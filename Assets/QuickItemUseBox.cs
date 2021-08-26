using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemBox))]
public class QuickItemUseBox : MonoBehaviour, IDropHandler
{
    public ItemBox itembox;
    public void LinkComponent()
    {
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
    internal void Init(int i, InventoryItemInfo inventoryItemInfo)
    {
        index = i;
        itembox.Init(inventoryItemInfo);
    }
}
