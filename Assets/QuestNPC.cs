using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] InputAction questAcceptKey;

    void Awake()
    {
        questAcceptKey.performed += QuestAcceptKey_performed;
    }

    void QuestAcceptKey_performed(InputAction.CallbackContext obj)
    {
        print("퀘스트 목록 UI 표시하기");
        QuestListUI.Instance.ShowQuestList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        print($"들어온거 누구야{other.name}");
        questAcceptKey.Enable();
        TalkAlertUI.Instance.ShowText("모험자야 멈춰봐!\n할 말이 있어(Q)");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        questAcceptKey.Disable();

    }
}
