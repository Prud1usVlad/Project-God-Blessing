﻿using Assets.Scripts.EventSystem;
using Assets.Scripts.SkillSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillNodeWidget : MonoBehaviour
{
    private bool isDisabled;
    private bool isOutranked;
    private bool inited = false;
    private IGameEventListener unselectedListener;

    public Skill skill;
    public GameEvent selectionEvent;

    public Image shader;
    public Image skillIcon;
    public TextMeshProUGUI topCornerText;

    public Color selectedShaderColor;
    public Color availableShaderColor;
    public Color disabledShaderColor;
    public Color outrankedShaderColor;

    public void Awake()
    {
        if (!inited)
            shader.color = outrankedShaderColor;
    }

    public void InitNode(Skill skill, bool isDisabled, bool isOutranked)
    {
        inited = true;
        this.skill = skill;
        this.isDisabled = isDisabled;
        this.isOutranked = isOutranked;

        skillIcon.sprite = skill.icon;
        
        switch(skill.topCornerIndicator)
        {
            case 0: topCornerText.SetText(""); break;
            case 1: topCornerText.SetText("I"); break;
            case 2: topCornerText.SetText("II"); break;
            case 3: topCornerText.SetText("III"); break;
            case 4: topCornerText.SetText("IV"); break;
            case 5: topCornerText.SetText("V"); break;
        }

        if (skill is not null && skill.isLearnd)
        {
            shader.color = new Color(0,0,0,0);
        }
        else
        {
            if (isDisabled)
            {
                shader.color = disabledShaderColor;
            }
            if (isOutranked || skill is null)
            {
                shader.color = outrankedShaderColor;
            }
            if (!isDisabled && !isOutranked &&
                skill is not null && !skill.isLearnd)
            {
                shader.color = availableShaderColor;
            }
        }
    }

    public void OnSelect()
    {
        if (skill is null)
            return;

        shader.color = selectedShaderColor;
        selectionEvent.Raise(skill.Guid);

        unselectedListener = new SimpleEventListener(OnSelectOther);
        selectionEvent.RegisterListener(unselectedListener);
    }

    public void OnSelectOther()
    {
        InitNode(skill, isDisabled, isOutranked);
        selectionEvent.UnregisterListener(unselectedListener);
    }
}
