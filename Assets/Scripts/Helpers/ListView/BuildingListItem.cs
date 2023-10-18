using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListItem : MonoBehaviour, IListItem            
{
    private bool isAvaliable = false;
    private bool isPlaced = false;
    private bool isReserched = false;

    public Building building;
    public GameProgress gameProgress;

    public GameEvent itemSelected;

    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDescription;
    public TooltipTrigger tooltipTrigger;
    public Transform resourcesPanel;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        building = data as Building;

        buildingName?.SetText(building?.buildingName);
        buildingDescription?.SetText(building?.description);

        GameObject pref = new GameObject("Resource");
        var text = pref.AddComponent<TextMeshProUGUI>();
        pref.AddComponent<TooltipTrigger>();
        text.fontSize = 24;

        foreach (var res in building.price.resources)
        {
            var item = Instantiate(pref, Vector3.zero, Quaternion.identity, resourcesPanel);
            
            item.GetComponent<TextMeshProUGUI>()
                .SetText($"{Enum.GetName(typeof(ResourceName), res.name)} : {res.amount}");
            
            item.GetComponent<TooltipTrigger>()
                .Init($"{gameProgress.resourceContainer.GetResourceAmount(res.name)}/{res.amount}");
        }

        Destroy(pref);
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
        isAvaliable = gameProgress.resourceContainer.CanAfford(building.price);
        isReserched = gameProgress.buildingResearch.Find(b => b.guid == building.Guid).isAvaliable;
        isPlaced = gameProgress.placedBuildings.Contains(building);

        if (isPlaced)
        {
            GetComponent<Image>().color = new Color(0, 1, 0, 0.5f);
            tooltipTrigger.Init("Building already placed", building.name);
        }
        else if (!isAvaliable || !isReserched)
        {
            GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
            
            var content = (!isReserched) ? "Building is not reserched yet"
                : "You don't have enough resources";
            tooltipTrigger.Init(content, building.name);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            tooltipTrigger.Init("Building is avaliable", building.name);
        }
    }

    public void OnSelecting()
    {
        if (isAvaliable && !isPlaced && isReserched)
        {
            Debug.Log("Select item " + name);
            itemSelected.Raise(building.Guid);
            Selection.Invoke();
        }
    }
}
