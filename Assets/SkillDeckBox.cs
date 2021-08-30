using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDeckBox : MonoBehaviour
{
    [SerializeField] Sprite enableSprite;
    [SerializeField] Sprite disableSprite;
    Image bgImage;
    Text skillName;
    Text skillLevel;
    Image icon;


    DeckStateType deckState;
    public void Init(DeckStateType _deckState)
    {
        bgImage = transform.Find("BG").GetComponent<Image>();
        skillName = transform.Find("SkillName").GetComponent<Text>();
        skillLevel = transform.Find("SkillLevel").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();
        skillName.enabled = false;
        skillLevel.enabled = false;
        icon.enabled = false;

        deckState = _deckState;
        bgImage.sprite = deckState == DeckStateType.Enable ?
                         enableSprite : disableSprite;

    }
}
