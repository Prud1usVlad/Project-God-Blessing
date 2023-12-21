using Assets.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillManagementWindow : DialogueBox
{
    public SkillSystem skillSystem;
    public ListViewController reserchedList;
    public List<EquipableSkillListItem> activeSkillSloths;
    public EquipableSkillListItem valueSloth;

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
        }

        return inited;
    }

    public void UpdateView()
    {
        reserchedList.InitView(skillSystem
            .learnedSkills.Cast<object>().ToList());

        valueSloth.FillItem(skillSystem.equipedValueSkill);
        foreach (var (active, i) in skillSystem
            .equipedActiveSkills.Select((value, i) => (value, i)))
        {
            activeSkillSloths[i].FillItem(active);
        }
    }

    public void EquipValue(GameObject item)
    {
        var skill = item
            .GetComponent<EquipableSkillListItem>()
            .skill as ValueSkill;
        skillSystem.Equip(skill);

        UpdateView();
    }

    public void EquipFirstActive(GameObject item) 
    {
        var skill = item
            .GetComponent<EquipableSkillListItem>()
            .skill as ActiveSkill;
        skillSystem.Equip(skill, 0);

        UpdateView();
    }

    public void EquipSecondActive(GameObject item)
    {
        var skill = item
            .GetComponent<EquipableSkillListItem>()
            .skill as ActiveSkill;
        skillSystem.Equip(skill, 1);

        UpdateView();
    }

    public void EquipThirdActive(GameObject item)
    {
        var skill = item
            .GetComponent<EquipableSkillListItem>()
            .skill as ActiveSkill;
        skillSystem.Equip(skill, 2);

        UpdateView();
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }

    public void OnEnable()
    {
        UpdateView();
    }
}
