using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class UserQuestData
{
    public List<int> acceptIDs = new List<int>();
    public List<int> rejectIDs = new List<int>();
    public int activeQuestID;
}
[System.Serializable]
public class InventoryItemInfo
{
    public int uid;
    public int id;
    public int count;
    [NonSerialized]
    public QuickSlotType type;
    public ItemInfo ItemInfo => ItemDB.GetItemInfo(id);

    public SkillInfo SkillInfo => ItemDB.GetSkillInfo(id);
}
[System.Serializable]
public class UserItemData : ISerializationCallbackReceiver
{
    public int lastUID;
    public List<InventoryItemInfo> item = new List<InventoryItemInfo>();
    public List<int> quickItemUIDs = new List<int>();
    public List<int> equipItemUIDs = new List<int>();

    public void OnAfterDeserialize()
    {
        if (quickItemUIDs.Count == 0)
            quickItemUIDs.AddRange(new int[10]);
        if (equipItemUIDs.Count == 0)
            equipItemUIDs.AddRange(new int[8]);
    }

    public void OnBeforeSerialize() { }
}
[System.Serializable]
public class AccountData : ISerializationCallbackReceiver
{
    public int gold;
    public int crystal;
    public string username;
    public int level = 1;
    public int exp;

    public void OnAfterDeserialize()
    {
        level = Math.Max(1, level);
    }

    public void OnBeforeSerialize() { }
}
[System.Serializable]
public class UserSkillInfo
{
    public int id;
    public int level;

    public SkillInfo SkillInfo => ItemDB.GetSkillInfo(id);
}
[System.Serializable]
public class SkillData : ISerializationCallbackReceiver
{
    public List<UserSkillInfo> skills = new List<UserSkillInfo>();

    public List<int> deckIDs = new List<int>(8);

    public void OnAfterDeserialize()
    {
        if (deckIDs.Count == 0)
            deckIDs.AddRange(new int[8]);
    }

    public void OnBeforeSerialize() { }
}
public enum QuickSlotType
{
    Item,
    Skill,
}
public class UserData : Singleton<UserData>
{
    // 수락, 거절한 퀘스트들
    public PlayerPrefsData<UserQuestData> questData;

    //내가 구입한 아이템
    public PlayerPrefsData<UserItemData> itemData;

    // 계정 정보
    public PlayerPrefsData<AccountData> accountData;

    // 스킬 정보
    public PlayerPrefsData<SkillData> skillData;

    internal Action<int, int> onChangedGold;

    private void Awake()
    {
        questData = new PlayerPrefsData<UserQuestData>("UserQuestData");
        itemData = new PlayerPrefsData<UserItemData>("UserItemData");
        accountData = new PlayerPrefsData<AccountData>("UserAccountData");
        skillData = new PlayerPrefsData<SkillData>("UserSkillData");
    }
    private void OnDestroy()
    {
        questData.SaveData();
        itemData.SaveData();
        accountData.SaveData();
        skillData.SaveData();
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

        return $"{item.name}, {count}개 구매 해따";
    }

    private bool IsEnoughGold(int needGold)
    {
        return accountData.data.gold >= needGold;
    }

    private void InsertItem(ItemInfo item)
    {
        var existItem = itemData.data.item
                        .Where(x => x.id == item.id && x.count < item.maxStackCount)
                        .FirstOrDefault();
        if (existItem != null)
        {
            existItem.count++;
            QuickSlotUI.Instance.UpdateItemInfo(existItem);
        }
        else
        {
            InventoryItemInfo newItem = new InventoryItemInfo();
            newItem.id = item.id;
            newItem.count = 1;
            newItem.uid = ++itemData.data.lastUID;
            itemData.data.item.Add(newItem);
        }
    }

    private void AddGold(int addGold)
    {
        int oldValue = accountData.data.gold;
        accountData.data.gold += addGold;
        onChangedGold?.Invoke(oldValue, accountData.data.gold);
    }
    private void SubGold(int subGold)
    {
        int oldValue = accountData.data.gold;
        accountData.data.gold -= subGold;
        onChangedGold?.Invoke(oldValue, accountData.data.gold);
    }

    internal List<InventoryItemInfo> GetItems(ItemType itemType)
    {
        return Instance.itemData.data.item
                       .Where(x => x.ItemInfo.itemType == itemType)
                       .ToList();
    }

    internal string ProcessSell(InventoryItemInfo item, int count)
    {
        int totalGold = item.ItemInfo.sellPrice * count;

        // 아이템 삭제
        RemoveItem(item, count);

        // 돈 추가
        AddGold(totalGold);

        return $"{item.ItemInfo.name}, {count}개 판매 해따";
    }

    public void RemoveItem(InventoryItemInfo item, int removeCount)
    {
        item.count -= removeCount;
        Debug.Assert(item.count >= 0, "0보다 작아질 수 없어");
        if (item.count == 0)
            itemData.data.item.Remove(item);
    }

    internal InventoryItemInfo GetItem(int itemUID)
    {
        return itemData.data.item.Where(x => x.uid == itemUID)
                                 .FirstOrDefault();
    }
}
