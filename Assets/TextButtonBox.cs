using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TextButtonBox : MonoBehaviour
{
    // public 권한이 아니면 Instantiate 했을 때 각 개체의 컴포넌트와 연결이 안됨
    public Button button;
    public Text text;
    public GameObject activeGo;
    public void LinkComponent()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        activeGo = transform.Find("Selected").gameObject;
    }
}
