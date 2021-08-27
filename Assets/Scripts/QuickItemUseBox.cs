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
        ItemBox fromItembox = eventData.pointerDrag.GetComponent<ItemBox>();

        int itemUID = fromItembox.inventoryItemInfo.uid;
        // 기존에 같은 UID 잇으면 해제하자
        QuickSlotUI.Instance.ClearSlot(itemUID);

        // 아이템 할당
        itembox.Init(fromItembox.inventoryItemInfo);

        // 할당하면 UserData에 저장
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
        // 정보 없으면 리턴
        if (itembox.inventoryItemInfo == null)
            return;

        // 쿨타임이 끝나지 않았으면 리턴
        if (Time.time < endTime)
            return;

        print(number.text);

        StartCoroutine(StartCoolTimeCo());
    }

    float endTime; //쿨타임 종료시간
    IEnumerator StartCoolTimeCo()
    {
        float coolTimeSeconds = 1;
        endTime = Time.time + coolTimeSeconds;
        while (Time.time < endTime)
        {
            float remainTime = endTime - Time.time;

            coolTimeText.text = remainTime.ToString("0.0");

            float remainPercent = remainTime / coolTimeSeconds; //1 -> 0
            coolTimeFilled.fillAmount = remainPercent;

            yield return null;
        }
        coolTimeText.text = "";
        coolTimeFilled.fillAmount = 0;

        transform.DOPunchScale(Vector3.one * 0.15f, 0.5f); 
    }
}
