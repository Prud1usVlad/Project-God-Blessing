using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Controllers.Hub.Buildings.Ui;
using UnityEngine;

public class ProductionBuildingDialogueBox : DialogueBox, IWorkersChecker
{
    private int workers;

    public BuildingController buildingController;
    public ProductionBuilding building;

    public TextMeshProUGUI workersText;
    public ListViewController productionList;

    public GameObject upgradePanel;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeName;
    public ListViewController upgradeResources;

    public override bool InitDialogue()
    {
        header = building.buildingName;
        body = building.description;

        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
        }

        return inited;
    }

    public void UpdateView(string recipeName = null)
    {
        var progress = buildingController.gameProgress;
        var production = progress.production.Where(p => p.buildingGuid == building.Guid);

        UpdateWorkers(production);
        productionList.InitView(production.Cast<object>().ToList());

        if (!buildingController.HasUpgrades())
        {
            upgradePanel.gameObject.SetActive(false);
        }
        else
        {
            if (!buildingController.CanUpgrade())
            {
                upgradeButton.interactable = false;
            }

            upgradeName.SetText(building.upgrade.buildingName);
            upgradeResources.InitView(building.upgrade
                .price.resources.Cast<object>().ToList());
        }

    }

    public bool ChekWorkers(string recipeName, int newValue)
    {
        var production = buildingController.gameProgress
            .production.Where(p => p.buildingGuid == building.Guid && p.recipe.name != recipeName);
        var newWorkers = 5 - production.Sum(p => p.workers) - newValue;

        return newWorkers >= 0;
    }

    public void UpdateWorkers(IEnumerable<Production> production = null) 
    {
        if (production == null)
        {
            var progress = buildingController.gameProgress;
            production = progress.production.Where(p => p.buildingGuid == building.Guid);
        }

        var workers = 5 - production.Sum(p => p.workers);
        workersText.SetText("Free workers: " + workers);
    }

    public void UpgradeBuilding()
    {
        buildingController.UpdgradeBuilding();
        modalManager.DialogueClose();
    }

    public override string GetHeader(string tag = null)
    {
        if (tag == "upgrade" && buildingController.HasUpgrades())
        {
            return building.upgrade.buildingName;
        }
        else
            return base.GetHeader(tag);
    }

    public override string GetContent(string tag = null)
    {
        if (tag == "upgrade" && buildingController.HasUpgrades())
        {
            return building.upgrade.description;
        }
        else
            return base.GetHeader(tag);
    }
}
