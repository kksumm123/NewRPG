using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/*
퀘스트 이름 : questTitle
상세내용 : detailExplain
목표 Type : enum QuestType
KillMonster,      // 몬스터 처치.
GoToDestination,  // 목적지 도착
ItemCollection, // 아이템 수집

GoalCount : 
몬스터 처치시는 몬스터 처치수
아이템 수집시는 아이템 수집수,

class Reward
{
ItemID
Count 
}*/

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
public class QuestListUI : Singleton<QuestListUI>
{
    CanvasGroup canvasGroup;
    public List<QuestInfo> quests;
    QuestTitleBox baseQuestTitleBox;
    RewardBox baseRewardBox;

    Text detailTitleText;
    Text detailContentText;
    Text detailGoalText;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        baseQuestTitleBox = GetComponentInChildren<QuestTitleBox>();
        baseRewardBox = GetComponentInChildren<RewardBox>();
        baseQuestTitleBox.Init();
        baseRewardBox.Init();
        detailTitleText = transform.Find("QuestDetail/Title").GetComponentInChildren<Text>();
        detailContentText = transform.Find("QuestDetail/Content").GetComponentInChildren<Text>();
        detailGoalText = transform.Find("QuestDetail/Goal/Text").GetComponent<Text>();
    }

    public void ShowQuestList()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        baseQuestTitleBox.gameObject.SetActive(true);
        foreach (var item in quests)
        {
            var titleItem = Instantiate(baseQuestTitleBox, baseQuestTitleBox.transform.parent);
            titleItem.Init(item);
            titleItem.GetComponent<Button>().onClick
                     .AddListener(() => OnClickTitleBox(item));
        }
        baseQuestTitleBox.gameObject.SetActive(false);

        OnClickTitleBox(quests[0]);
    }

    void OnClickTitleBox(QuestInfo item)
    {
        detailTitleText.text = item.questTitle;
        detailContentText.text = item.detailExplain;
        detailGoalText.text = item.GetGoalString();
    }
}
