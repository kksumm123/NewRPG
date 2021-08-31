using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDeckBox : MonoBehaviour, IDropHandler
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
    public SkillInfo skillInfo;
    public void OnDrop(PointerEventData eventData)
    {
        // eventData.pointerDrag = 드래그가 시작된 오브제트
        skillInfo = eventData.pointerDrag.GetComponent<SkillListBox>().skillInfo;
        
        if (skillInfo != null)
            SetUI(skillInfo);
    }

    private void SetUI(SkillInfo skillInfo)
    {
        icon.sprite = skillInfo.Sprite;
        skillName.text = skillInfo.name;

        int level = 0;
        var userSkillInfo = UserData.Instance.skillData.data.skills
                                    .Find(x => x.id == skillInfo.id);
        if (userSkillInfo != null)
            level = userSkillInfo.level;

        print(level);
        skillLevel.text = level.ToString();
        SetActiveUI(true);
    }

    private void SetActiveUI(bool state)
    {
        skillName.enabled = skillLevel.enabled = icon.enabled = state;
    }
}
