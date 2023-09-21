using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.ListView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingListItem : MonoBehaviour, IListItem            
{
    public Building building;

    public GameEvent itemSelected;

    public TextMeshProUGUI buildingName;
    public TextMeshProUGUI buildingDescription;

    public void FillItem(object data)
    {
        building = (Building)data;

        buildingName?.SetText(building?.name);
        buildingDescription?.SetText(building?.description);
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
        Debug.Log("Select item " + name);
        itemSelected.Raise(building.Guid);
    }

    public void OnUnselected()
    {
        Debug.Log("Unselect item " + name);
    }
}
