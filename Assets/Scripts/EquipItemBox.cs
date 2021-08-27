using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemBox))]
public class EquipItemBox : MonoBehaviour, IDropHandler
{
    public int index;
    public ItemBox itemBox;
    public void OnDrop(PointerEventData eventData)
    {
        print(eventData);
        ItemBox fromItemBox = eventData.pointerDrag.GetComponent<ItemBox>();
        itemBox.Init(fromItemBox.inventoryItemInfo);

        // 할당하면 UserData에 저장
        int itemUID = fromItemBox.inventoryItemInfo.uid;
        UserData.Instance.itemData.data.equipItemUIDs[index] = itemUID;
    }
    internal void Init(int i, InventoryItemInfo inventoryItemInfo)
    {
        index = i;
        itemBox.Init(inventoryItemInfo);
    }

    internal void LinkComponent()
    {
        itemBox = GetComponent<ItemBox>();
        itemBox.LinkComponent();
    }
}
