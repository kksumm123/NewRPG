using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public abstract class NPC : MonoBehaviour
{
    [SerializeField] InputAction showUIKey = 
        new InputAction("Key", InputActionType.Button, "<Keyboard>/q");
    [SerializeField] protected string speechString = "모험자야 멈춰봐!\n할 말이 있어(Q)";
    [SerializeField] protected string npcName = "NPC";
    [SerializeField] protected string npcSpriteName = "NPC1";

    void Awake()
    {
        showUIKey.performed += ShowYiKey_performed;
    }
    private void OnDestroy()
    {
        showUIKey.performed -= ShowYiKey_performed;
    }

    void ShowYiKey_performed(InputAction.CallbackContext obj)
    {
        print("UI 표시하기");
        CharacterTextBoxUI.Instance.CloseUI();
        ShowUI();
    }

    protected abstract void ShowUI();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        // 유저에게 보여줄 퀘스트가 있을 때만 진행하자
        // 보여줄 퀘스트 : 수락/완료/거절한 퀘스트 제외
        if (IsUseableMenu() == false)
            return;

        showUIKey.Enable();
        CharacterTextBoxUI.Instance.ShowText(speechString, 3, npcName, npcSpriteName);
    }

    protected virtual bool IsUseableMenu()
    {
        Debug.LogError("이거 표시되면 안됨, 자식에서 오버라이드 필수");
        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        showUIKey.Disable();
        CharacterTextBoxUI.Instance.CloseUI();
    }
}
public class QuestNPC : NPC
{
    [SerializeField] List<int> questIDs = new List<int>();

    protected override bool IsUseableMenu()
    {
        List<int> ignoreIDs = new List<int>();

        ignoreIDs.AddRange(UserData.Instance.questData.data.acceptIDs);
        ignoreIDs.AddRange(UserData.Instance.questData.data.rejectIDs);
        return questIDs.Where(x => ignoreIDs.Contains(x) == false).Count() > 0;
    }

    protected override void ShowUI()
    {
        QuestListUI.Instance.ShowQuestList(questIDs);
    }
}
