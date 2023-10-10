using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeWindow : MonoBehaviour
{
    public SkillSystem skillSystem;
    public NationName nation;

    public Slider progressionSlider;
    public TextMeshProUGUI freePoints;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI skillType;
    
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

    }
    
    private void UpdateView()
    {
        FillProgress(translation);

        freePoints.SetText(translation.freeResearchPoints.ToString());
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