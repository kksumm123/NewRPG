using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] InputAction questAcceptKey;
    [SerializeField] List<int> questIDs = new List<int>();

    void Awake()
    {
        questAcceptKey.performed += QuestAcceptKey_performed;
    }

    void QuestAcceptKey_performed(InputAction.CallbackContext obj)
    {
        print("퀘스트 목록 UI 표시하기");
        QuestListUI.Instance.ShowQuestList(questIDs);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        // 유저에게 보여줄 퀘스트가 있을 때만 진행하자
        // 보여줄 퀘스트 : 수락/완료/거절한 퀘스트 제외
        if (HaveUseableQuest() == false)
            return;

        questAcceptKey.Enable();
        TalkAlertUI.Instance.ShowText("모험자야 멈춰봐!\n할 말이 있어(Q)");
    }

    private bool HaveUseableQuest()
    {
        List<int> ignoreIDs = new List<int>();

        ignoreIDs.AddRange(UserData.Instance.questData.data.acceptIDs);
        ignoreIDs.AddRange(UserData.Instance.questData.data.rejectIDs);
        return questIDs.Where(x => ignoreIDs.Contains(x) == false).Count() > 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        questAcceptKey.Disable();

    }
}
