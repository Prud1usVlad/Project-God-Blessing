using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.QuestSystem;
using Assets.Scripts.SkillSystem.Ui;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoticeBoardDialogueBox : DialogueBox
{
    [SerializeField]
    private GameProgress gameProgress;
    private Quest selected;

    public ListViewController quests;
    public GameObject secretsWindow;

    [Header("Info panel properties")]
    public GameObject infoPanel;

    public TextMeshProUGUI questName;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI level;
    public TextMeshProUGUI nation;
    
    public Slider progress;
    public TextMeshProUGUI progressText;

    public TextMeshProUGUI connectionProgressHeader;
    public AdditiveSliderWidget connectionProgress;
    public AdditiveSliderWidget fameProgress;

    public ListViewController resRewards;
    public ListViewController itemRewards;

    public ListViewController stages;
    public Button acceptBtn;
    public Button collectBtn;
    public Button declineBtn;

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
            UpdateView();

        return inited;
    }

    public void UpdateView()
    {
        var data = gameProgress.questSystem
            .availableQuests.Cast<object>().ToList();

        quests.InitView(data, data.FirstOrDefault());

        quests.selectionChanged += UpdateInfo;

        if (data.Count == 0)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
            UpdateInfo();
        }
    }

    public void UpdateInfo()
    {
        selected = quests.Selected as Quest;

        UpdateTextData(selected);
        UpdateListViews(selected);
        UpdateAdditiveSliders(selected);
        UpdateButtons(selected);
    }

    public void OnAcceptQuest() 
    {
        gameProgress.questSystem.AcceptQuest(selected);
        UpdateView();
    }

    public void OnDeclineQuest()
    {
        gameProgress.questSystem.DeclineQuest(selected);
        UpdateView();
    }

    public void OnCollectRewards()
    {
        gameProgress.questSystem.CollectRewards(selected);
        UpdateView();
    }

    public void OnManageSecrets()
    {
        var dialogue = Instantiate(secretsWindow, transform)
            .GetComponent<SecretsPostingWindow>();

        modalManager.DialogueOpen(dialogue);
    }

    private void UpdateButtons(Quest quest)
    {
        switch (quest.status)
        {
            case QuestStatus.Available:
                declineBtn.gameObject.SetActive(true);
                acceptBtn.gameObject.SetActive(true);
                collectBtn.gameObject.SetActive(false);
                break;
            case QuestStatus.InProgress:
                declineBtn.gameObject.SetActive(true);
                acceptBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(false);
                break;
            case QuestStatus.Completed:
                declineBtn.gameObject.SetActive(false);
                acceptBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(true);
                break;
        }
    }

    private void UpdateListViews(Quest quest)
    {
        resRewards.InitView(quest.data
            .resources.Cast<object>().ToList());
        itemRewards.InitView(quest.data.equipment
            .Select(e => new InventoryRecord(e, quest.data.connectionLevel))
            .Cast<object>().ToList());

        var stagesToShow = quest.stages
            .Where(s => s.isCompleted || s == quest.stage);

        stages.InitView(stagesToShow.Cast<object>().ToList());
    }

    private void UpdateTextData(Quest quest)
    {
        questName.SetText(quest.data.questName);
        questDescription.SetText(quest.data.description);

        level.SetText(quest.data.connectionLevel + "lvl.");
        nation.SetText(Enum.GetName(typeof(NationName), quest.data.nation));

        progress.value = quest.progress;
        progressText.SetText(Math.Round(quest.progress * 100, 1) + "%");
    }

    private void UpdateAdditiveSliders(Quest quest)
    {
        var currFame = gameProgress.fameTranslation.currentPoints;

        fameProgress.UpdateView(
            currFame,
            currFame + quest.data.famePoints,
            Converters.NationToColor(quest.data.nation),
            true,
            gameProgress.fameTranslation.GetPointsForNextLevel() + currFame,
            gameProgress.fameTranslation.currentFameLevel.points
        );

        if (quest.data.nation != NationName.None)
        {
            var currConn = gameProgress.skillSystem
                .connections.GetConnection(quest.data.nation);

            connectionProgressHeader.gameObject.SetActive(true);
            connectionProgress.transform.
                parent.gameObject.SetActive(true);

            connectionProgress.UpdateView(
                currConn.currentPoints,
                currConn.currentPoints + quest.data.connectionPoints,
                Converters.NationToColor(quest.data.nation),
                true,
                currConn.GetLevelPoints(currConn.currentLevelIdx + 1),
                currConn.GetLevelPoints(currConn.currentLevelIdx)
            );
        }
        else
        {
            connectionProgressHeader.gameObject.SetActive(false);
            connectionProgress.transform
                .parent.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameProgress.questSystem.AddProgress(new()
            {
                { "amount", 1 }
            });

            UpdateView();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            gameProgress.questSystem.FillAvailable();
            UpdateView();
        }
    }
}