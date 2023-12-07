using Assets.Scripts.ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionBuildingDialogueBox : DialogueBox
{
    private List<ConsumptionWidget> consumptionWidgets;

    public BuildingController buildingController;
    public ProductionBuilding building;

    public ProductionWidget productionWidget;
    public Transform consumptionPanel;
    
    public Slider powerSlider;

    public GameObject consumptionPrefab;
    public Button upgradeButton;

    public override bool InitDialogue()
    {
        header = building.buildingName;
        body = building.description;
        powerSlider.value = building.productionPower;
        consumptionWidgets = new List<ConsumptionWidget>();

        var inited = base.InitDialogue();

        if (inited)
        {
            UpdateView();
            StartCoroutine(SliderCheckRoutine());
        }

        return inited;
    }

    public void UpdateView()
    {
        string resName = Enum.GetName(typeof(ResourceName), building.resource.name);
        string resProd = Mathf.RoundToInt(building.resource.amount *
            building.productionMultiplier * building.productionPower) + "/day";

        productionWidget.UpdateView(resName, resProd);

        var resources = building.productionPrice.resources;

        if (consumptionWidgets.Count == 0)
        {
            foreach(var r in resources) 
            {
                var widget = Instantiate(consumptionPrefab, consumptionPanel)
                    .GetComponent<ConsumptionWidget>();
                consumptionWidgets.Add(widget);
            }
        }

        for (int i = 0; i < resources.Count; i++)
        {
            string name = Enum.GetName(typeof(ResourceName), building.productionPrice.resources[i].name);
            string point = building.productionPrice.resources[i].amount + "/item";
            string all = building.productionPrice.resources[i].amount * building.productionPower + "/day";

            consumptionWidgets[i].UpdateView(name, point, all);
        }

        if (!buildingController.HasUpgrades()) 
        {
            upgradeButton.gameObject.SetActive(false);
        }
        else if (!buildingController.CanUpgrade())
        {
            upgradeButton.interactable = false;
        }
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
            if (buildingController.CanUpgrade())
                return "Building is ready for upgrade";
            else
                return "Can't upgrade";
        }
        else
            return base.GetHeader(tag);
    }

    private IEnumerator SliderCheckRoutine()
    {
        while(true)
        {
            if (powerSlider.value != building.productionPower)
            {
                building.productionPower = (int)powerSlider.value;
                UpdateView();
            }

            yield return new WaitForSeconds(0.5f);
        }
    } 
}
