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

    public Building building;
    public ResourceContainer resourceContainer;

    public GameEvent itemSelected;

    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDescription;

    public Transform resourcesPanel;

    public void FillItem(object data)
    {
        building = (Building)data;

        buildingName?.SetText(building?.name);
        buildingDescription?.SetText(building?.description);

        foreach (var res in building.price.resources)
        {
            GameObject obj = new GameObject("Resource");
            var text = obj.AddComponent<TextMeshProUGUI>();

            text.SetText($"{Enum.GetName(typeof(ResourceName), res.name)} : {res.amount}");

            obj.transform.SetParent(resourcesPanel);
        }

        isAvaliable = resourceContainer.CanAfford(building.price);
        if (!isAvaliable)
        {
            GetComponent<Image>().color = new Color (1, 0, 0, 0.5f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
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
        if (isAvaliable)
        {
            Debug.Log("Select item " + name);
            itemSelected.Raise(building.Guid);
        }
    }

    public void OnUnselected()
    {
        Debug.Log("Unselect item " + name);
    }
}
