using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListBox : MonoBehaviour
{
    Text skillName;
    Text skillMana;
    Image icon;

    public SkillInfo skillInfo;
    public void Init(SkillInfo _skillInfo)
    {
        skillInfo = _skillInfo;
        skillName = transform.Find("SkillName").GetComponent<Text>();
        skillMana = transform.Find("SkillMana").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();
        if (skillInfo != null)
        {
            skillName.text = skillInfo.name;
            skillMana.text = skillInfo.mana.ToString();
            icon.sprite = skillInfo.Sprite;
        }
        else
        {
            skillName.text = string.Empty;
            skillMana.text = string.Empty;
            icon.sprite = null;
        }
    }
}
