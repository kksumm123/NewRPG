﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int questID;
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
    List<GameObject> questTitleBoxs = new List<GameObject>();
    List<GameObject> rewardBoxs = new List<GameObject>();

    Text detailTitleText;
    Text detailContentText;
    Text detailGoalText;

    CanvasGroup npcTalkBoxCanvasGroup;
    Text npcTalkBoxText;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        baseQuestTitleBox = GetComponentInChildren<QuestTitleBox>();
        baseRewardBox = GetComponentInChildren<RewardBox>();
        baseQuestTitleBox.LinkComponent();
        baseRewardBox.LinkComponent();
        detailTitleText = transform.Find("QuestDetail/Title").GetComponentInChildren<Text>();
        detailContentText = transform.Find("QuestDetail/Content").GetComponentInChildren<Text>();
        detailGoalText = transform.Find("QuestDetail/Goal/TextParent/Text").GetComponent<Text>();
        npcTalkBoxCanvasGroup = transform.Find("NPCTalkBox").GetComponent<CanvasGroup>();
        npcTalkBoxCanvasGroup.alpha = 0;
        npcTalkBoxText = transform.transform.Find("NPCTalkBox/Text").GetComponent<Text>();
        transform.Find("NPCTalkBox/Accept").GetComponent<Button>().onClick
                                           .AddListener(() => AcceptQuest());
        transform.Find("NPCTalkBox/Reject").GetComponent<Button>().onClick
                                           .AddListener(() => RejectQuest());
        transform.Find("CloseButton/Button").GetComponent<Button>().onClick
                                           .AddListener(() => CloseUI());
    }

    private void AcceptQuest()
    {
        print($"{currentQuest.questTitle} 퀘스트 수락함");
        UserData.Instance.questData.data.acceptIDs.Add(currentQuest.questID);

        ShowQuestList();
    }

    private void RejectQuest()
    {
        print($"{currentQuest.questTitle} 퀘스트 거절함");
        UserData.Instance.questData.data.rejectIDs.Add(currentQuest.questID);
    }

    private void CloseUI()
    {
        canvasGroup.DOFade(0, 0.5f);
    }

    public void ShowQuestList()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        // 퀘스트 목록
        questTitleBoxs.ForEach(x => Destroy(x));
        questTitleBoxs.Clear();

        baseQuestTitleBox.gameObject.SetActive(true);

        // 수락, 거절한 퀘스트 제거
        List<int> exceptIDs = new List<int>();
        exceptIDs.AddRange(UserData.Instance.questData.data.acceptIDs);
        exceptIDs.AddRange(UserData.Instance.questData.data.rejectIDs);
        var userQuestList = quests.Where(x => exceptIDs.Contains(x.questID) == false).ToList();

        if (userQuestList.Count > 0)
        {
            foreach (var item in userQuestList)
            {
                var titleItem = Instantiate(baseQuestTitleBox, baseQuestTitleBox.transform.parent);
                titleItem.Init(item);
                titleItem.GetComponent<Button>().onClick
                         .AddListener(() => OnClickTitleBox(item));
                questTitleBoxs.Add(titleItem.gameObject);
            }

            // 첫번째 퀘스트 선택
            OnClickTitleBox(userQuestList[0]);
        }
        else
            ClearUI();

        baseQuestTitleBox.gameObject.SetActive(false);
    }

    private void ClearUI()
    {
        currentQuest = null;
        npcTalkBoxText.text = string.Empty;
        detailTitleText.text = string.Empty;
        detailContentText.text = string.Empty;
        detailGoalText.text = string.Empty;

        rewardBoxs.ForEach(x => Destroy(x));
        rewardBoxs.Clear();
    }

    QuestInfo currentQuest;
    void OnClickTitleBox(QuestInfo item)
    {
        currentQuest = item;
        npcTalkBoxText.text = $"{item.questTitle} 퀘스트 수락할래?";
        npcTalkBoxCanvasGroup.alpha = 0;
        npcTalkBoxCanvasGroup.DOFade(1, 0.5f);
        detailTitleText.text = item.questTitle;
        detailContentText.text = item.detailExplain;
        detailGoalText.text = item.GetGoalString();

        //보상 목록
        rewardBoxs.ForEach(x => Destroy(x));
        rewardBoxs.Clear();

        baseRewardBox.gameObject.SetActive(true);
        foreach (var rewardItem in item.rewards)
        {
            var titleItem = Instantiate(baseRewardBox, baseRewardBox.transform.parent);
            titleItem.Init(rewardItem);
            rewardBoxs.Add(titleItem.gameObject);
        }
        baseRewardBox.gameObject.SetActive(false);
    }
}
