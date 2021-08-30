using DG.Tweening;
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
    public Text coolTimeText;
    public Image coolTimeFilled;
    public void LinkComponent()
    {
        number = transform.Find("Number").GetComponent<Text>();
        itembox = GetComponent<ItemBox>();
        itembox.LinkComponent();
        coolTimeText = transform.Find("CoolTimeText").GetComponent<Text>();
        coolTimeFilled = transform.Find("CoolTimeFilled").GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        print(eventData);
        var fromQuickItemUseBox = eventData.pointerDrag.GetComponent<QuickItemUseBox>();
        if (fromQuickItemUseBox)
        {
            // 스왑 바꾸기
            // 기존에 있던거랑 fromQuickItemUseBox랑 바꾸자

            // 기존 itembox의 정보를 저장하자
            var thisInventoryItemInfo = itembox.inventoryItemInfo;
            var fromInventoryItemInfo = fromQuickItemUseBox.itembox.inventoryItemInfo;
            // 기존껄 바꾸자
            SetIconAndSaveSlotData(fromInventoryItemInfo
                                , fromInventoryItemInfo != null ? fromInventoryItemInfo.uid : 0
                                , itembox
                                , index);

            // From에 있는걸 바꾸자
            SetIconAndSaveSlotData(thisInventoryItemInfo
                                , thisInventoryItemInfo != null ? thisInventoryItemInfo.uid : 0
                                , fromQuickItemUseBox.itembox
                                , fromQuickItemUseBox.index);
        }
        else
        {
            // ItemBox 관련해서 쓰기 위해서는
            // ItemBox, QuickSlotUI, UserData 3개의 컴포넌트를 가져와야한다
            // 그러므로 인터페이스를 사용하는 편이 좋다
            // 인터페이스는 어떻게 쓰는거지?
            ItemBox fromItembox = eventData.pointerDrag.GetComponent<ItemBox>();

            int itemUID = fromItembox.inventoryItemInfo.uid;
            // 기존에 같은 UID 잇으면 해제하자
            QuickSlotUI.Instance.ClearSlot(itemUID);
            SetIconAndSaveSlotData(fromItembox.inventoryItemInfo, itemUID, itembox, index);
        }
    }

    void SetIconAndSaveSlotData(InventoryItemInfo setInventoryItemInfo, int SaveitemUID
                                , ItemBox itembox, int index)
    {
        // 아이템 할당
        itembox.Init(setInventoryItemInfo);

        // 할당하면 UserData에 저장
        UserData.Instance.itemData.data.quickItemUIDs[index] = SaveitemUID;
    }

    public int index;
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
        // 정보 없으면 리턴
        if (itembox.inventoryItemInfo == null)
            return;

        // 쿨타임이 끝나지 않았으면 리턴
        if (Time.realtimeSinceStartup < endTime)
            return;

        print(number.text);

        // 소비 아이템인가?
        bool isConsumable = itembox.inventoryItemInfo.ItemInfo.itemType == ItemType.Consume;
        if (isConsumable)
        {
            // 소비 아이템인 경우 수량을 줄이자
            UserData.Instance.RemoveItem(itembox.inventoryItemInfo, 1);

            // 개수가 0이면 itembox 비우자
            if (itembox.inventoryItemInfo.count <= 0)
                itembox.inventoryItemInfo = null;

            // 바뀐 정보로 다시 Init()
            itembox.Init(itembox.inventoryItemInfo);
        }
        StartCoroutine(StartCoolTimeCo());
    }

    float endTime; //쿨타임 종료시간
    IEnumerator StartCoolTimeCo()
    {
        float coolTimeSeconds = 1;
        endTime = Time.realtimeSinceStartup + coolTimeSeconds;
        while (Time.realtimeSinceStartup < endTime)
        {
            float remainTime = endTime - Time.realtimeSinceStartup;

            coolTimeText.text = remainTime.ToString("0.0");

            float remainPercent = remainTime / coolTimeSeconds; //1 -> 0
            coolTimeFilled.fillAmount = remainPercent;

            yield return null;
        }
        coolTimeText.text = "";
        coolTimeFilled.fillAmount = 0;

        transform.DOPunchScale(Vector3.one * 0.15f, 0.5f).SetUpdate(true);
    }
}
