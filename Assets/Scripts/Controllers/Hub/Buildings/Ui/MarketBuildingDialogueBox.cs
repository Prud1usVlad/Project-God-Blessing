using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.TooltipSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketBuildingDialogueBox : DialogueBox
{
    [NonSerialized]
    public MarketBuilding building;
    [NonSerialized]
    public InventoryRecord selectedItem;

    public ListViewController itemsList;
    public GameObject detailsPanel;
    public InventoryItemTooltip selectedDetails;
    public ListViewController selectedPrice;
    public Button buyButton;
    
    public override bool InitDialogue()
    {
        header = building.buildingName;
        body = building.description;

        var inited = base.InitDialogue();
        itemsList.selectionChanged += OnSelectionChanged;
        
        if (inited) UpdateView();
            return inited;
    }

    public void UpdateView()
    {
        itemsList.InitView(building.items.Cast<object>().ToList());

        UpdateDetails();
    }

    public void UpdateDetails()
    {
        if (selectedItem == null)
        {
            detailsPanel.SetActive(false);
        }
        else
        {
            detailsPanel.SetActive(true);

            selectedDetails.InitView(selectedItem);
            selectedPrice.InitView(selectedItem.item
                .buyPrice.resources.Cast<object>().ToList());

            buyButton.enabled = building.CanAffordItem(selectedItem);
        }
    }

    public void OnBuySelected()
    {
        building.BuyItem(selectedItem);

        UpdateView();
        selectedItem = null;
    }

    public override string GetContent(string tag = null)
    {
        switch (tag.ToUpper())
        {
            case "PRICE":
                return GetPriceContent();
            default : return base.GetContent(tag);
        }
    }

    public override string GetHeader(string tag = null)
    {
        switch (tag.ToUpper())
        {
            case "PRICE":
                return "";
            default: return base.GetContent(tag);
        }
    }

    private string GetPriceContent()
    {
        var res = new StringBuilder();
        var prog = building.gameProgress;

        foreach (var resource in selectedItem.item.buyPrice.resources) 
        {
            res.AppendLine($"{Enum.GetName(typeof(ResourceName), resource.name)}" +
                $" {prog.resourceContainer.GetResourceAmount(resource.name)}" +
                $" / {resource.amount}");
        }

        return res.ToString();
    }

    private void OnSelectionChanged()
    {
        selectedItem = itemsList.Selected as InventoryRecord;
        UpdateDetails();
    }
}
