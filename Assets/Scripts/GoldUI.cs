using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    Text value;
    float animationDuration = 0.5f;
    void Start()
    {
        value = transform.Find("Value").GetComponent<Text>();
        UserData.Instance.onChangedGold += ChangedGold;
        ChangedGold(0, UserData.Instance.accountData.data.gold);
    }

    void ChangedGold(int oldValue, int newValue)
    {
        DOTween.To(() => oldValue, (x) =>value.text = x.ToString()
                    , newValue, animationDuration)
               .SetUpdate(true).SetLink(gameObject);
    }
}
