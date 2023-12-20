using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListItem : MonoBehaviour, IListItem            
{
    private bool isAvailable = false;
    private bool isPlaced = false;
    private bool isReserched = false;

    public Building building;
    public GameProgress gameProgress;

    public GameEvent itemSelected;

    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDescription;
    public TooltipTrigger tooltipTrigger;
    public ListViewController resources;

    public Color defaultColor;
    public Color availableColor;
    public Color notAvaliableColor;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        building = data as Building;

        buildingName?.SetText(building?.buildingName);
        buildingDescription?.SetText(building?.description);

        resources.InitView(building.price
            .resources.Cast<object>().ToList());

        ApplyColors();
    }

    public bool HasData(object data)
    {
        if (data is Building)
        {
            return (data as Building)  == building;
        }
        else return false;
    }

    public void OnSelected()
    {
        
    }

    private void ApplyColors()
    {
        isAvailable = gameProgress.resourceContainer.CanAfford(building.price);
        isReserched = gameProgress.buildingResearch.Find(b => b.guid == building.Guid).isAvailable;
        isPlaced = gameProgress.placedBuildings.Contains(building);

        if (isPlaced)
        {
            GetComponent<Image>().color = defaultColor;
            tooltipTrigger.Init("Building already placed", building.name);
        }
        else if (!isAvailable || !isReserched)
        {
            GetComponent<Image>().color = notAvaliableColor;
            
            var content = (!isReserched) ? "Building is not reserched yet"
                : "You don't have enough resources";
            tooltipTrigger.Init(content, building.name);
        }
        else
        {
            GetComponent<Image>().color = availableColor;
            tooltipTrigger.Init("Building is available", building.name);
        }
    }

    public void OnSelecting()
    {
        if (isAvailable && !isPlaced && isReserched)
        {
            Debug.Log("Select item " + name);
            itemSelected.Raise(building.Guid);
            Selection.Invoke();
        }
    }
}
