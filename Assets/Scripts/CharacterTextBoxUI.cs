using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTextBoxUI : Singleton<CharacterTextBoxUI>
{
    Image portrait;
    Text nameText;
    Text contentsText;
    CanvasGroup canvasGroup;
    void Awake()
    {
        portrait = transform.Find("Portrait").GetComponent<Image>();
        nameText = transform.Find("Name/TextParent/Text").GetComponent<Text>();
        contentsText = transform.Find("TextParent/Text").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    float speechSpeed = 20;
    public void ShowText(string _text, float visibleTime = 3
        , string _name = "NPC", string portraitSpriteName = "NPC1")
    {
        gameObject.SetActive(true);
        contentsText.DOKill();
        canvasGroup.DOKill();

        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);

        // 모든 텍스트 띄우기
        //contentsText.text = _text;
        // 한 글자씩 말하듯이 띄우기
        contentsText.text = "";
        contentsText.DOText(_text, _text.Length / speechSpeed).SetDelay(0.3f).SetUpdate(true);
        nameText.text = _name;
        portrait.sprite = Resources.Load<Sprite>("NPCs/" + portraitSpriteName);
        portrait.SetNativeSize();
        CloseUI().SetDelay(visibleTime).SetUpdate(true);
    }

    public Tweener CloseUI()
    {
        return canvasGroup.DOFade(0, 0.5f)
                          .SetUpdate(true)
                          .OnComplete(() => gameObject.SetActive(false));
    }
}
