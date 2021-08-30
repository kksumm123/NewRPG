﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseUI<T> : Singleton<T> where T : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StageManager.GameState = GameStateType.Menu;
    }
    void OnDisable()
    {
        //if (Application. == true)
        //return;
        StageManager.GameState = GameStateType.Play;
    }
    public virtual void ShowUI()
    {
        if (gameObject.activeSelf)
            return;

        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);
    }
    public void CloseUI()
    {
        canvasGroup.DOFade(0, 0.5f).SetUpdate(true)
                   .OnComplete(() => gameObject.SetActive(false));
    }
}
public class SkillUI : BaseUI<SkillUI>
{

    public override void ShowUI()
    {
        base.ShowUI();

        if (isCompleteLink == false)
            LinkComponent();
    }

    bool isCompleteLink = false;
    SkillDeckBox baseSkillDeckBox;
    List<SkillDeckBox> skillDeckBoxs = new List<SkillDeckBox>();
    private void LinkComponent()
    {
        InitDeck();
        InitSkillList();
    }

    private void InitSkillList()
    {
        throw new NotImplementedException();
    }

    void InitDeck()
    {
        // 초기화 하자
        baseSkillDeckBox = GetComponentInChildren<SkillDeckBox>(true);

        // 레벨 1, 5개 사용가능
        // 2 : 6, 3 : 7, 4 : 8, ... 
        int level = UserData.Instance.accountData.data.level;
        for (int i = 0; i < 8; i++)
        {
            DeckStateType deckState = 4 + level > i ? DeckStateType.Enable : DeckStateType.Disable;
            var newbox = Instantiate(baseSkillDeckBox, baseSkillDeckBox.transform.parent);
            newbox.Init(deckState);
            skillDeckBoxs.Add(newbox);
        }
        baseSkillDeckBox.gameObject.SetActive(false);
        isCompleteLink = true;
    }
}
public enum DeckStateType
{
    Disable,
    Enable,
    Used,
}