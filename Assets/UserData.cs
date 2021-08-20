using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserQuestData
{
    public List<int> acceptIDs;
    public List<int> rejectIDs;
}
public class UserData : Singleton<UserData>
{
    public PlayerPrefsData<UserQuestData> QuestData;
    private void Awake()
    {
        QuestData = new PlayerPrefsData<UserQuestData>("UserQuestData");
    }
    private void OnDestroy()
    {
        QuestData.SaveData();
    }
}
