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
public class UserData : Singleton<UserData>
{
    // 수락, 거절한 퀘스트들
    public PlayerPrefsData<UserQuestData> questData;

    //내가 구입한 아이템
    public PlayerPrefsData<UserItemData> itemData;
    private void Awake()
    {
        questData = new PlayerPrefsData<UserQuestData>("UserQuestData");
        itemData = new PlayerPrefsData<UserItemData>("UserItemData");
    }
    private void OnDestroy()
    {
        questData.SaveData();
        itemData.SaveData();
    }
}
