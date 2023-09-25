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

    public Transform resourcesPanel;

    public void FillItem(object data)
    {
        building = (Building)data;

        buildingName?.SetText(building?.buildingName);
        buildingDescription?.SetText(building?.description);

        foreach (var res in building.price.resources)
        {
            GameObject obj = new GameObject("Resource");
            var text = obj.AddComponent<TextMeshProUGUI>();

            text.SetText($"{Enum.GetName(typeof(ResourceName), res.name)} : {res.amount}");

            obj.transform.SetParent(resourcesPanel);
        }

        ApplyColors();
    }

    public bool HasData(object data)
    {
        if (data is Building)
        {
            return (data as Building) == building;
        }
        else return false;
    }

    public void OnSelected()
    {
        if (isAvaliable && !isPlaced && isReserched)
        {
            Debug.Log("Select item " + name);
            itemSelected.Raise(building.Guid);
        }
    }

    public void OnUnselected()
    {
        Debug.Log("Unselect item " + name);
    }

    private void ApplyColors()
    {
        isAvaliable = gameProgress.resourceContainer.CanAfford(building.price);
        isReserched = gameProgress.buildingResearch.Find(b => b.guid == building.Guid).isAvaliable;
        isPlaced = gameProgress.placedBuildings.Contains(building);

        if (isPlaced)
        {
            GetComponent<Image>().color = new Color(0, 1, 0, 0.5f);
        }
        else if (!isAvaliable || !isReserched)
        {
            GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }
}
