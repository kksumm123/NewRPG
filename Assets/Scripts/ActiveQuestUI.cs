using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveQuestUI : Singleton<ActiveQuestUI>
{
    public TextButtonBox baseQuestBox;
    void Awake()
    {
        baseQuestBox = transform.Find("BG/QuestBox").GetComponent<TextButtonBox>();
        baseQuestBox.LinkComponent();
    }
    void Start()
    {
        RefreshQuestList();
    }

    public List<GameObject> questBoxs = new List<GameObject>();
    public void RefreshQuestList()
    {
        var activeIds = UserData.Instance.questData.data.acceptIDs;
        var activeQuests = ItemDB.Instance.GetQuestInfo(activeIds);

        questBoxs.ForEach(x => Destroy(x));
        questBoxs.Clear();

        baseQuestBox.gameObject.SetActive(true);
        int activeID = UserData.Instance.questData.data.activeQuestID;
        foreach (var item in activeQuests)
        {
            var newQuestBox = Instantiate(baseQuestBox, baseQuestBox.transform.parent);
            newQuestBox.text.text = item.questTitle;

            newQuestBox.activeGo.SetActive(activeID == item.id);
            newQuestBox.button.onClick.AddListener(() => OnClick(item));

            questBoxs.Add(newQuestBox.gameObject);
        }
        baseQuestBox.gameObject.SetActive(false);
    }

    private void OnClick(QuestInfo item)
    {
        UserData.Instance.questData.data.activeQuestID = item.id;
        RefreshQuestList();
    }
}
