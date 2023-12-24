using Assets.Scripts.Helpers;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EconomicsWindow : DialogueBox
{
    private Resource selected;

    private float prevBuyVal = 0;
    private float prevSellVal = 0;
    private bool canAfford = true;

    public GameObject resourcePrefab;
    public GameObject messageBox;

    public ResourceContainer container;
    public ResourceMarket market;

    public ListViewController listView;

    [Header("Market ui elements")]
    public Slider sellSlider;
    public Slider buySlider;
    public TextMeshProUGUI sellAmount;
    public TextMeshProUGUI buyAmount;
    public TextMeshProUGUI sellPrice;
    public TextMeshProUGUI buyPrice;
    public GameObject marketPanel;
    public Button confirmTransactionButton;

    [Header("Info panel ui elements")]
    public TextMeshProUGUI gainedMarket;
    public TextMeshProUGUI gainedProduction;
    public TextMeshProUGUI gainedLoot;
    public TextMeshProUGUI gainedOther;
    public TextMeshProUGUI gainedAll;

    public TextMeshProUGUI spentMarket;
    public TextMeshProUGUI spentProduction;
    public TextMeshProUGUI spentBuild;
    public TextMeshProUGUI spentOther;
    public TextMeshProUGUI spentAll;

    public override bool InitDialogue()
    {
        var inited = base.InitDialogue();

        if (inited)
        {
            listView.selectionChanged += OnSelectionChanged;

            selected = container.Resources.First();
            listView.InitView(container.Resources.Cast<object>().ToList(), selected);

            UpdateView();
        }

        return inited;
    }

    public void OnClose()
    {
        modalManager.DialogueClose();
    }

    public void ConfirmTransaction()
    {
        int buyVal = (int)buySlider.value;
        int sellVal = (int)sellSlider.value;

        var trans = new ResourceDynamic
        { gained = buyVal, spent = sellVal };

        if (market.CheckDynamic(selected.name, trans))
        {

            var canAffordToBuy = container.CanAfford(
                market.GetBuyPrice(selected.name, buyVal));
            var canAffordToSell = container.CanAfford(
                market.GetSellPrice(selected.name, sellVal));

            if (canAffordToBuy && canAffordToSell)
            {
                market.SetTransaction(selected.name, trans);

                var header = "Transaction confirmed";
                var body = $"Now on you will be buying {buyVal} and " +
                    $"selling {sellVal} of {Enum.GetName(typeof(ResourceName), selected.name)} per day";
                ShowMessage(header, body);
            }
            else if (!canAffordToBuy)
            {
                var header = "Can't afford to buy";
                var body = "You don't have enough gold to buy resources";
                ShowMessage(header, body);
            }
            else if (!canAffordToSell)
            {
                var header = "Can't afford to sell";
                var body = "You don't have enough resources to sell";
                ShowMessage(header, body);
            }
        }
    }

    public void OnSelectionChanged()
    {
        UpdateView();

        if (selected.name == ResourceName.Gold)
            marketPanel.SetActive(false);
        else
            marketPanel.SetActive(true);


    }

    private void UpdateView()
    {
        listView.RefreshList();
        selected = listView.Selected as Resource;
        UpdateInfo();

        if (selected.name != ResourceName.Gold)
            OnResourceChanged();

    }

    public void UpdateInfo()
    {
        var stats = container.GetStatistics(selected.name);
        
        gainedMarket.SetText("Gained in market: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.ResourceMarket)?.gained ?? 0));
        spentMarket.SetText("Spent in market: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.ResourceMarket)?.spent ?? 0));

        gainedProduction.SetText("Gained in production: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.Production)?.gained ?? 0));
        spentProduction.SetText("Spent in production: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.Production)?.spent ?? 0));

        gainedLoot.SetText("Gained in dungeons: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.Loot)?.gained ?? 0));

        spentBuild.SetText("Spent in build: " + Converters
            .IntToUiString(GetStat(stats, TransactionType.Build)?.spent ?? 0));

        var all = new ResourceDynamic() { gained = 0, spent = 0 };
        var other = new ResourceDynamic() { gained = 0, spent = 0 };
        
        var gainOtherCategory = new List<TransactionType>
        {
                TransactionType.Sell, TransactionType.Build,
                TransactionType.Cashback, TransactionType.Any
        };
        var spentOtherCategory = new List<TransactionType>
        {
                TransactionType.Sell, TransactionType.Loot,
                TransactionType.Cashback, TransactionType.Any
        };

        foreach (var item in stats)
        {
            all.spent += item.Value.spent;
            all.gained += item.Value.gained;

            if (gainOtherCategory.Contains(item.Key))
                other.gained += item.Value.gained;
            if (spentOtherCategory.Contains(item.Key))
                other.spent += item.Value.spent;
        }

        gainedAll.SetText("Gained all: " + Converters.IntToUiString(all.gained));
        spentAll.SetText("Spent all: " + Converters.IntToUiString(all.spent));

        gainedOther.SetText("Gained other: " + Converters.IntToUiString(other.gained));
        spentOther.SetText("Spent other: " + Converters.IntToUiString(other.spent));
    }

    private void UpdateMarket()
    {
        var conf = market.GetConfig(selected.name);

        sellSlider.maxValue = conf.maxSellAmount;
        buySlider.maxValue = conf.maxBuyAmount;

        sellAmount.SetText(sellSlider.value + "pt.");
        buyAmount.SetText(buySlider.value + "pt.");

        var sPrice = market.GetSellPrice(selected.name, (int)sellSlider.value);
        var bPrice = market.GetBuyPrice(selected.name, (int)buySlider.value);

        int goldGain = container.GetResource(ResourceName.Gold)
            .ValueWithModifiers(sPrice.resources[0].amount * conf.sellPrice,
                TransactionType.ResourceMarket, true);
        int goldSpent = container.GetResource(ResourceName.Gold)
            .ValueWithModifiers(bPrice.resources[0].amount,
                TransactionType.ResourceMarket, false);

        var canAffordS = container.CanAfford(sPrice);
        var canAffordB = container.CanAfford(bPrice);
        canAfford = canAffordB && canAffordS;

        if (canAfford)
            confirmTransactionButton.interactable = true;
        else
            confirmTransactionButton.interactable = false;


        sellPrice.SetText($"+{Converters.IntToUiString(goldGain)}$");
        buyPrice.SetText($"-{Converters.IntToUiString(goldSpent)}$");

        if (canAffordB)
            buyPrice.color = new Color(0, 0.5f, 0);
        else
            buyPrice.color = new Color(0.5f, 0, 0);

        if (canAffordS)
            sellPrice.color = new Color(0, 0.5f, 0);
        else
            sellPrice.color = new Color(0.5f, 0, 0);

        prevSellVal = sellSlider.value;
        prevBuyVal = buySlider.value;
    }

    private void OnResourceChanged()
    {
        UpdateMarket();
        var transaction = market.GetTransaction(selected.name);

        sellSlider.value = transaction.spent;
        buySlider.value = transaction.gained;
    }

    private void ShowMessage(string header, string body)
    {
        var box = Instantiate(messageBox, transform)
            .GetComponentInChildren<DialogueBox>();

        box.header = header;
        box.body = body;

        modalManager.DialogueOpen(box);
    }

    private ResourceDynamic GetStat
        (Dictionary<TransactionType, ResourceDynamic> dict, TransactionType transaction) 
    { 
        dict.TryGetValue(transaction, out var resource);
        return resource;
    }

    private void Update()
    {
        if (prevBuyVal != buySlider.value
            || prevSellVal != sellSlider.value)
        {
            UpdateMarket();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            container.GainResource(ResourceName.Gold, 100, TransactionType.Loot);
            listView.RefreshList();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            container.SpendResource(ResourceName.Gold, 100, TransactionType.Sell);
            listView.RefreshList();
        }
    }

}
