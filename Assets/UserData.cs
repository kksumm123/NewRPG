using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserQuestData
{
    public List<int> acceptIDs = new List<int>();
    public List<int> rejectIDs = new List<int>();
}
[System.Serializable]
public class UserItemData
{
    public List<InventoryItemInfo> item = new List<InventoryItemInfo>();
}
[System.Serializable]
public class AccountData
{
    public int gold;
    public int crystal;
    public string username;
}
public class UserData : Singleton<UserData>
{
    // 수락, 거절한 퀘스트들
    public PlayerPrefsData<UserQuestData> questData;

    //내가 구입한 아이템
    public PlayerPrefsData<UserItemData> itemData;

    // 계정 정보
    public PlayerPrefsData<AccountData> accountData;

    private void Awake()
    {
        questData = new PlayerPrefsData<UserQuestData>("UserQuestData");
        itemData = new PlayerPrefsData<UserItemData>("UserItemData");
        accountData = new PlayerPrefsData<AccountData>("UserAccountData");
    }
    private void OnDestroy()
    {
        questData.SaveData();
        itemData.SaveData();
        accountData.SaveData();
    }

    internal string ProcessBuy(ItemInfo item, int count)
    {
        int totalGold = item.buyPrice * count;
        // 돈이 있는지 확인
        if (IsEnoughGold(totalGold) == false)
            return "소지금이 부족합니다";

        // 아이템 지금
        for (int i = 0; i < count; i++)
        {
            InsertItem(item);
        }

        // 소지금 - 가격
        SubGold(totalGold);

        return $"{item.name}, {count} 구매 해따";
    }

    private bool IsEnoughGold(int needGold)
    {
        return accountData.data.gold >= needGold;
    }

    private void InsertItem(ItemInfo item)
    {
        throw new NotImplementedException();
    }

    private void SubGold(int totalGold)
    {
        throw new NotImplementedException();
    }
}
