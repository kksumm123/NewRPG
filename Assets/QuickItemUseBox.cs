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
    }
}
