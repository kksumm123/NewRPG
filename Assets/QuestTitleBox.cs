﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTitleBox : MonoBehaviour
{
    public Text titleText;
    public void Init()
    {
        titleText = transform.Find("Text").GetComponent<Text>();
    }

    public void Init(QuestInfo item)
    {
        titleText.text = item.questTitle;
    }
}
