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
    void Start()
    {
        portrait = transform.Find("Portrait").GetComponent<Image>();
        nameText = transform.Find("Name/TextParent/Text").GetComponent<Text>();
        contentsText = transform.Find("TextParent/Text").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowText(string _text, float visibleTime = 3
        , string _name = "NPC", string portraitSpriteName = "NPC1")
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        contentsText.text = _text;
        nameText.text = _name;
        portrait.sprite = Resources.Load<Sprite>("NPCs/" + portraitSpriteName);

        canvasGroup.DOFade(0, 0.5f).SetDelay(visibleTime);
    }

    public void Close()
    {
        canvasGroup.DOFade(0, 0.5f);
    }
}
