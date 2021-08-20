using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UserQuestData
{
    public List<int> acceptIDs = new List<int>();
    public List<int> rejectIDs = new List<int>();
}
public class UserData : Singleton<UserData>
{
    public PlayerPrefsData<UserQuestData> questData;
    private void Awake()
    {
        questData = new PlayerPrefsData<UserQuestData>("UserQuestData");
    }
    private void OnDestroy()
    {
        questData.SaveData();
    }
}
