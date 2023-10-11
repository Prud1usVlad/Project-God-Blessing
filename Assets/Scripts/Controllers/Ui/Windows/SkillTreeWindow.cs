using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.SkillSystem;

public class SkillTreeWindow : MonoBehaviour
{
    public SkillSystem skillSystem;
    public NationName nation;

    [Header("Simple ui components")]
    public Slider progressionSlider;
    public TextMeshProUGUI freePoints;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillType;
    public Button learnButton;

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
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void InitWindow(NationName nation)
    {
        this.nation = nation;

        UpdateView();

        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }

    public void OnLearnSkill()
    {

    }

    public void OnSelectSkill(string skillGuid)
    {
        var skill = skills.FindByGuid(skillGuid);

        skillName.SetText(skill.skillName);
        skillDescription.SetText(skill.description);
        skillType.SetText(System.Enum.GetName(typeof(SkillType), skill.type));

        var allReqLearned = skill.required.All(s => s.isLearnd);

        var isO = translation.IsSkillOutranked(skill);
        var isD = !(allReqLearned && translation.IsSkillAvaliable(skill));

        learnButton.interactable = !(isO || isD);

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
            var isD = !(allReqLearned && translation.IsSkillAvaliable(skill));

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