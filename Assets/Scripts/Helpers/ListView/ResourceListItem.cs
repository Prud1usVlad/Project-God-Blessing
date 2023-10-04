using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using TMPro;
using UnityEngine;

public class ResourceListItem : MonoBehaviour, IListItem
{
    public Resource resource;

    public TextMeshProUGUI _name;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI gained;
    public TextMeshProUGUI used;

    public ResourceContainer resourceContainer;

    public string resName;

    public void FillItem(object data)
    {
        resource = (Resource)data;
        resName = Enum.GetName(typeof(ResourceName), resource.name);

        _name.SetText(resName);
        amount.SetText(Converters.IntToUiString(resource.amount));
        
        gained.SetText(Converters.IntToUiString(
            resourceContainer.GaindedPerDay(resource.name)));
        used.SetText(Converters.IntToUiString(
            resourceContainer.SpentPerDay(resource.name)));
    }

    public bool HasData(object data)
    {
        if (data is Resource)
        {
            return (data as Resource) == resource;
        }
        else return false;
    }

    public void OnSelected()
    {
    }

    public void OnUnselected()
    {
        Debug.Log(resName);
    }

}
