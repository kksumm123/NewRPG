using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDeckBox : MonoBehaviour, IDropHandler
{

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite enableSprite;
    [SerializeField] Sprite disableSprite;
    Image bgImage;
    Text skillName;
    Text skillLevel;
    Image icon;


    DeckStateType deckState;
    int index;
    public void Init(int _index, DeckStateType _deckState)
    {
        index = _index;
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
        UserData.Instance.skillData.data.deckIDs[index] = skillInfo.id;
        if (skillInfo != null)
            SetUI(skillInfo);
    }

    public void SetUI(SkillInfo _skillInfo)
    {
        skillInfo = _skillInfo;
        if (_skillInfo != null)
        {
            icon.sprite = _skillInfo.Sprite;
            skillName.text = _skillInfo.name;

            var userSkillInfo = UserData.Instance.skillData.data.skills
                                        .Find(x => x.id == _skillInfo.id);
            if (userSkillInfo != null)
                skillLevel.text = $"Lv.{userSkillInfo.level}";
            else
                skillLevel.text = "미획득";

            bgImage.sprite = normalSprite;
            SetActiveUI(true);
        }
        else
        {
            icon.sprite = null;
            skillName.text = string.Empty;
            SetActiveUI(false);
            bgImage.sprite = deckState == DeckStateType.Enable ?
                         enableSprite : disableSprite;
        }
    }

    private void SetActiveUI(bool state)
    {
        skillName.enabled = skillLevel.enabled = icon.enabled = state;
    }
}
