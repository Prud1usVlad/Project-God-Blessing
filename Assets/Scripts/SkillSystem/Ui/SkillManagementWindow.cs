using Assets.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillManagementWindow : MonoBehaviour
{
    public SkillSystem skillSystem;
    public ListViewController reserchedList;
    public List<EquipableSkillListItem> activeSkillSloths;
    public EquipableSkillListItem valueSloth;

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
        Destroy(gameObject);
    }

    public void OnEnable()
    {
        UpdateView();
    }
}
