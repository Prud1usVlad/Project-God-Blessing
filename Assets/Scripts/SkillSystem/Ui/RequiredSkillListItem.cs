using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.SkillSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequiredSkillListItem : MonoBehaviour, IListItem
{
    private Skill skill;

    public TextMeshProUGUI skillName;
    public Image indicator;
    public Color learnedColor;
    public Color notLearnedColor;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        skill = (Skill)data;

        skillName.SetText(skill.skillName);

        if (skill.isLearnd)
            indicator.color = learnedColor;
        else
            indicator.color = notLearnedColor;
    }

    public bool HasData(object data)
    {
        return skill.Equals(data);
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
    }
}