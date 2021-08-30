using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListBox : MonoBehaviour
{
    Text skillName;
    Text skillMana;
    Image icon;
    public void Init(SkillInfo skillInfo)
    {
        skillName = transform.Find("SkillName").GetComponent<Text>();
        skillMana = transform.Find("SkillMana").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();

        skillName.text = skillInfo.name;
        skillMana.text = skillInfo.mana.ToString();
        icon.sprite = skillInfo.Sprite;
    }
}
