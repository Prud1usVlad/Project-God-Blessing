using System;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.SkillSystem;
using UnityEngine;
using UnityEngine.UI;

public class EquipableSkillListItem : MonoBehaviour, IListItem
{
    public SkillSystem skillSystem;

    public bool isActiveSkill;
    public Skill skill;
    public Image underlay;
    public Image shade; 
    public TooltipTrigger trigger;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        if (data == null) return;

        isActiveSkill = data is ActiveSkill;
        skill = data as Skill;

        if (isActiveSkill)
            underlay.color = new Color(0, 1, 0, 0.5f);
        else underlay.color = new Color(1, 0.92f, 0.016f, 0.5f);

        if (skillSystem.IsEquiped(skill))
            shade.gameObject.SetActive(true);
        else
            shade.gameObject.SetActive(false);

        trigger.header = skill.skillName;
        trigger.content = skill.description;
    }

    public bool HasData(object data)
    {
        return data as Skill == skill;
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
        Selection?.Invoke();
    }
}