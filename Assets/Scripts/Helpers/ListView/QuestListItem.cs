using Assets.Scripts.Helpers.ListView;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.QuestSystem;
using Assets.Scripts.Helpers;
using Assets;
using Assets.Scripts.TooltipSystem;

public class QuestListItem :  TooltipDataProvider, IListItem
{
    [SerializeField]
    private GameProgress gameProgress;
    private Quest quest;

    public TextMeshProUGUI header;
    public TextMeshProUGUI content;
    public TextMeshProUGUI level;
    public TextMeshProUGUI nation;

    public GameObject progressHeader;
    public Slider progress;
    public AdditiveSliderWidget connections;

    public Image underlay;

    public Color acceptedQuestUnderlayColor;
    public Color completedQuestUnderlayColor;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        quest = data as Quest;

        header.SetText(quest.data.questName);
        content.SetText(quest.data.description);
        level.SetText(quest.data.connectionLevel + "lvl.");

        InitSliders();

        if (quest.status == QuestStatus.InProgress)
        {
            underlay.color = acceptedQuestUnderlayColor;
        }
        else if (quest.status == QuestStatus.Completed)
        {
            underlay.color = completedQuestUnderlayColor;
        }
    }

    private void InitSliders()
    {
        if (quest.status == QuestStatus.InProgress)
        {
            progress.gameObject.SetActive(true);
            progressHeader.SetActive(true);
            progress.value = quest.progress;
        }
        else if (quest.status == QuestStatus.Completed)
        {
            progress.gameObject.SetActive(true);
            progressHeader.SetActive(true);
            progress.value = quest.progress;
            progress.fillRect.GetComponent<Image>().color = 
                completedQuestUnderlayColor;
        }
        else
        {
            progress.gameObject.SetActive(false);
            progressHeader.SetActive(false);
        }

        if (quest.data.nation == NationName.None)
        {
            connections.gameObject.SetActive(false);
            nation.gameObject.SetActive(false);
        }
        else
        {
            connections.gameObject.SetActive(true);
            nation.gameObject.SetActive(true);
            nation.SetText(Enum.GetName(typeof(NationName), quest.data.nation));

            var connection = gameProgress.skillSystem
                .connections.GetConnection(quest.data.nation);

            float a = connection.currentPoints;
            float b = connection.currentPoints + quest.data.connectionPoints;
            float max = connection
                .GetLevelPoints(connection.currentLevelIdx + 1);
            float min = connection
                .GetLevelPoints(connection.currentLevelIdx);

            var color = Converters.NationToColor(quest.data.nation);
            connections.UpdateView(a, b, color, false, max, min);
        }
    }

    public bool HasData(object data)
    {
        return data as Quest == quest;
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
        Selection.Invoke();
    }

    public override string GetHeader(string tag = null)
    {
        return quest.data.name;
    }

    public override string GetContent(string tag = null)
    {
        return quest.data.description;
    }
}