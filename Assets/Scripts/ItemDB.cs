using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestType
{
    KillMonster,      // 몬스터 처치.
    GoToDestination,  // 목적지 도착
    ItemCollection, // 아이템 수집
}
[System.Serializable]
public class RewardInfo
{
    public int itemID;
    public int count;
}

[System.Serializable]
public class QuestInfo
{
    public string questTitle;
    public int id;
    [TextArea]
    public string detailExplain;
    public QuestType questType;

    /// <summary>
    /// 몬스터 처치시는 몬스터 ID
    /// 아이템 수집시는 아이템 ID,</summary>
    public int goalId;
    /// <summary>
    /// 몬스터 처치시는 몬스터 처치수
    /// 아이템 수집시는 아이템 수집수,</summary>
    public int goalCount;

    public List<RewardInfo> rewards;

    internal string GetGoalString()
    {
        switch (questType)
        {
            case QuestType.KillMonster: // 슬라임 5마리 처치하세요
                string monsterName = ItemDB.GetMonsterInfo(goalId).name;
                return $"{monsterName}\n{goalCount}마리";
            case QuestType.GoToDestination: // 촌장님 댁으로 이동하세요
                string destrinationName = ItemDB.GetDestinationInfo(goalId).name;
                return $"{destrinationName}";
            case QuestType.ItemCollection: // 보석을 5개 수집하세요
                string itemName = ItemDB.GetItemInfo(goalId).name;
                return $"{itemName}\n{goalCount}개";
        }
        return "임시 작업해야함";
    }
}
[System.Serializable]
public class MonsterInfo
{
    public string name;
    public int id;
}
[System.Serializable]
public class DestinationInfo
{
    public string name;
    public int id;
}
[System.Serializable]
public class SkillInfo
{
    public int id;
    public string name;
    public int mana;
    [TextArea]
    public string description;
    public string icon;

    public Sprite Sprite => Resources.Load<Sprite>($"Icons/{icon}");
}
[System.Serializable]
public class ItemInfo
{
    public string name;
    public int id;
    public string iconName;
    [TextArea]
    public string description;
    public Sprite Sprite => Resources.Load<Sprite>($"Icons/{iconName}");
    public int sellPrice; // 상점에 팔 때
    public int buyPrice; // 상점에서 구입할 때
    public bool registAtShop; // 상점에 등록할 지, 말 지
    public ItemType itemType;
    public int maxStackCount = 1; // 한 슬롯에 최대 몇개 쌓이는지
}
public enum ItemType
{
    Money = 0,      // 재화       id : 0 ~ 10
    Weapon,         // 무기       id : 1001 ~ 2000
    Armor,          // 방어구     id : 2001 ~ 3000
    Accessory,      // 장신구     id : 3001 ~ 4000
    Consume,        // 소비아이템  id : 4001 ~ 5000
    Material,       // 재료       id : 5001 ~ 5000
    Etc,            // 기타       id : 6001 ~ 6000
}
public class ItemDB : Singleton<ItemDB>
{
    public List<QuestInfo> quests;
    [SerializeField] List<ItemInfo> items;
    [SerializeField] List<MonsterInfo> monsters;
    [SerializeField] List<DestinationInfo> destinations;
    public List<SkillInfo> skills;
    Dictionary<int, QuestInfo> questMap;
    Dictionary<int, ItemInfo> itemMap;
    Dictionary<int, MonsterInfo> monsterMap;
    Dictionary<int, DestinationInfo> destinationMap;
    Dictionary<int, SkillInfo> skillMap;
    private void Awake()
    {
        questMap = quests.ToDictionary(x => x.id);
        monsterMap = monsters.ToDictionary(x => x.id);
        destinationMap = destinations.ToDictionary(x => x.id);
        itemMap = items.ToDictionary(x => x.id);
        skillMap = skills.ToDictionary(x => x.id);
    }

    internal static QuestInfo GetQuestInfo(int questID)
    {
        if (Instance.questMap.TryGetValue(questID, out QuestInfo result) == false)
            Debug.LogError($"{questID}가 없다");
        return result;
    }

    internal List<QuestInfo> GetQuestInfo(List<int> questIDs)
    {
        //List<QuestInfo> result = new List<QuestInfo>(questIDs.Count);
        //foreach (var item in questIDs)
        //{
        //    result.Add(GetQuestInfo(item));
        //}
        //return result;
        return quests.Where(x => questIDs.Contains(x.id)).ToList();
    }

    internal static MonsterInfo GetMonsterInfo(int monsterID)
    {
        if (Instance.monsterMap.TryGetValue(monsterID, out MonsterInfo result) == false)
            Debug.LogError($"{monsterID}가 없다");
        return result;
    }
    internal static DestinationInfo GetDestinationInfo(int destinationID)
    {
        if (Instance.destinationMap.TryGetValue(destinationID, out DestinationInfo result) == false)
            Debug.LogError($"{destinationID}가 없다");
        return result;
    }
    internal static ItemInfo GetItemInfo(int itemID)
    {
        if (Instance.itemMap.TryGetValue(itemID, out ItemInfo result) == false)
            Debug.LogError($"{itemID}가 없다");
        return result;
    }

    internal List<ItemInfo> GetItems(ItemType itemType)
    {
        return items.Where(x => x.itemType == itemType).ToList();
    }
}
