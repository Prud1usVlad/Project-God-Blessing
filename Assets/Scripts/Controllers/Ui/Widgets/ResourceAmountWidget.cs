using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.TooltipSystem;
using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAmountWidget : TooltipDataProvider, IListItem
{
    public ResourceDescriptions descriptions;
    public Resource resource;

    public TextMeshProUGUI amount;
    public Image icon;

    public Action Selection { get; set; }

    public void FillItem(object data)
    {
        UpdateView(data as Resource);
    }

    public override string GetContent(string tag = null)
    {
        return descriptions.GetResourceDescription(resource.name);
    }

    public override string GetHeader(string tag = null)
    {
        return descriptions.GetResourceHumanName(resource.name);
    }

    public bool HasData(object data)
    {
        return this.resource == data;
    }

    public void OnSelected()
    {
    }

    public void OnSelecting()
    {
    }

    public void UpdateView(Resource resource)
    {
        this.resource = resource;
        amount.SetText("X" + Converters.IntToUiString(resource.amount));
        icon.sprite = descriptions.GetResourceIcon(resource.name);
    }
}
