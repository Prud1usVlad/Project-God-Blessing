using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SkillSystem;
using Assets.Scripts.SaveSystem;

public class SkillTreeWindow : DialogueBox
{
    private Skill selected = null;

    public SkillSystem skillSystem;
    public NationName nation;
    public GameObject skillManagementWindowPrefab;

    [Header("Simple ui components")]
    public GameObject info;
    public Slider progressionSlider;
    public TextMeshProUGUI freePoints;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillType;
    public Button learnButton;
    public ListViewController requiredSkills;
    public GameObject noSkillsReqIndicator;

    [Header("Skill node parents")]
    public GameObject buildings;
    public GameObject active;
    public GameObject secrets;
    public GameObject values;
    public GameObject devices;
    
    public ConnectionsContainer connections => 
        skillSystem.connections;
    public ConnectionsTranslation translation =>
        skillSystem.connections.GetConnection(nation);
    public SkillRegistry skills => 
        skillSystem.skillRegistries.Find(r => r.nation == nation);

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();

            info.SetActive(false);
        }

        return inited;
    }

    public void InitData(NationName nation)
    {
        this.nation = nation;
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }

    public void OnManageSkills()
    {
        var dialogue = Instantiate(skillManagementWindowPrefab, transform)
            .GetComponent<SkillManagementWindow>();
    
        modalManager.DialogueOpen(dialogue);
    }

    public void OnLearnSkill()
    {
        if (selected is not null)
        {
            skillSystem.LearnSkill(nation, selected.Guid);
            UpdateView();
            info.SetActive(false);
        }
    }

    public void OnSelectSkill(string skillGuid)
    {
        info.SetActive(true);
        var skill = skills.FindByGuid(skillGuid);
        selected = skill;

        skillName.SetText(skill.skillName);
        skillDescription.SetText(skill.description);
        skillType.SetText(System.Enum.GetName(typeof(SkillType), skill.type));

        if (skill.required.Count > 0)
            noSkillsReqIndicator.SetActive(false);
        else
            noSkillsReqIndicator.SetActive(true);

        requiredSkills.InitView(skill.required.Cast<object>().ToList());

        var allReqLearned = skill.required.All(s => s.isLearnd);

        var isO = translation.IsSkillOutranked(skill);
        var isD = !(allReqLearned && translation.IsSkillAvailable(skill));

        learnButton.interactable = !(isO || isD) && !skill.isLearnd;

    }
    
    private void UpdateView(bool updateNodes = true)
    {
        FillProgress(translation);
        if(updateNodes) InitAllNodes();

        freePoints.SetText(translation.freeResearchPoints.ToString());
    }

    private void InitAllNodes()
    {
        InitNodes(buildings, SkillType.Building);
        InitNodes(active, SkillType.Active);
        InitNodes(secrets, SkillType.Secret);
        InitNodes(values, SkillType.Value);
        InitNodes(devices, SkillType.Device);
    }

    private void InitNodes(GameObject nodesParent, SkillType skillType)
    {
        var nodes = nodesParent.GetComponentsInChildren<SkillNodeWidget>().ToList();
        var skills = this.skills.skills
            .Where(s => s.type == skillType)
            .OrderBy(s => s.level).ToList();

        for (int i = 0; i < skills.Count(); i++)
        {
            var skill = skills[i];
            var allReqLearned = skill.required.All(s => s.isLearnd);

            var isO = translation.IsSkillOutranked(skill);
            var isD = !(allReqLearned && translation.IsSkillAvailable(skill));

            nodes[i].InitNode(skill, isD, isO);
        }
    }

    private void FillProgress(ConnectionsTranslation translation)
    {
        float currPoints = translation.currentPoints;
        float nededPoints = translation.GetPointsForNextLevel() + currPoints;

        float sliderPart = 1f / translation.levelsAmount;
        float sliderFillAmount = sliderPart * translation.currentLevelIdx;
        float lowerVal = translation.GetLevelPoints(translation.currentLevelIdx);

        sliderFillAmount += ((currPoints - lowerVal) * sliderPart) / (nededPoints - lowerVal);

        progressionSlider.value = sliderFillAmount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            connections.AddPoints(20, nation);
            UpdateView();
        }
    }
}