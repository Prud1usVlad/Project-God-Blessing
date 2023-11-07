using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.QuestSystem.Stages;
using Assets.Scripts.TooltipSystem;
using System;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.QuestSystem;

public class QuestStageListItem : TooltipDataProvider, IListItem
{
    public StageInstance stage;

    public Image overlay;
    public TextMeshProUGUI stageName;
    public TextMeshProUGUI stageDescription;
    public Slider progress;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        stage = data as StageInstance;

        if (stage.isCompleted)
        {
            overlay.gameObject.SetActive(true);
            progress.gameObject.SetActive(false);
        }
        else
        {
            overlay.gameObject.SetActive(false);
            progress.gameObject.SetActive(true);

            progress.value = stage.progress;
        }

        stageName.SetText(stage.shortDescription);
        stageDescription.SetText(stage.longDescription);
    }

    public bool HasData(object data)
    {
        return stage.Equals(data);
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
    }

    public override string GetHeader(string tag = null)
    {
        if (stage.data is KillStage)
            return "Kill stage";
        else if (stage.data is CollectStage)
            return "Collect stage";
        else if (stage.data is TravelStage)
            return "Travel stage";
        else if (stage.data is InteractStage)
            return "Interact stage";
        else 
            return "";
    }

    public override string GetContent(string tag = null)
    {
        return stage.longDescription;
    }
}
