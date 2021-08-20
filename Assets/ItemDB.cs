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
public class ItemInfo
{
    public string name;
    public int id;
    public string iconName;
    public Sprite Sprite => Resources.Load<Sprite>($"Icons/{iconName}");
}
public class ItemDB : Singleton<ItemDB>
{
    [ContextMenu("퀘스트옮기기")]
    void CopyQuest()
    {
        quests = FindObjectOfType<QuestListUI>().quests;
    }
    [SerializeField] List<QuestInfo> quests;
    [SerializeField] List<ItemInfo> items;
    [SerializeField] List<MonsterInfo> monsters;
    [SerializeField] List<DestinationInfo> destinations;
    Dictionary<int, QuestInfo> questMap;
    Dictionary<int, ItemInfo> itemMap;
    Dictionary<int, MonsterInfo> monsterMap;
    Dictionary<int, DestinationInfo> destinationMap;
    private void Awake()
    {
        questMap = quests.ToDictionary(x => x.id);
        monsterMap = monsters.ToDictionary(x => x.id);
        destinationMap = destinations.ToDictionary(x => x.id);
        itemMap = items.ToDictionary(x => x.id);
    }

    internal static QuestInfo GetQuestInfo(int questID)
    {
        if (Instance.questMap.TryGetValue(questID, out QuestInfo result) == false)
            Debug.LogError($"{questID}가 없다");
        return result;
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

}
