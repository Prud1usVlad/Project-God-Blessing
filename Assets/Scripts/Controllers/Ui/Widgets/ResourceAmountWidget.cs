using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using System;
using TMPro;
using UnityEngine;

public class ResourceAmountWidget : MonoBehaviour, IListItem
{
    public Resource data;

    public TextMeshProUGUI resource;
    public TextMeshProUGUI amount;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        UpdateView(data as Resource);
    }

    public bool HasData(object data)
    {
        return this.data == data;
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
    }

    public void UpdateView(Resource res)
    {
        this.data = res;
        resource.SetText(Enum.GetName(typeof(ResourceName), res.name));
        amount.SetText(Converters.IntToUiString(res.amount));
    }
}
