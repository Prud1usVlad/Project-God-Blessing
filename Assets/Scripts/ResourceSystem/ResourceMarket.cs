using Assets.Scripts.ResourceSystem;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;
using System.Linq;

[CreateAssetMenu(fileName = "ResourceMarket", menuName = "ScriptableObjects/ResourceSystem/Market")]
public class ResourceMarket : ScriptableObject
{
    private Dictionary<ResourceName, ResourceDynamic> weeklyTransactions;

    public List<ResourceMarketConfig> resourceConfigs;

    public ResourceMarketConfig GetConfig(ResourceName resource)
    {
        return resourceConfigs.Find(c => c.resource == resource);
    }

    public ResourceDynamic GetTransaction(ResourceName resource)
    {
        if (weeklyTransactions.ContainsKey(resource))
            return weeklyTransactions[resource];
        else
            return new ResourceDynamic() { gained = 0, spent = 0 };
    }

    // Returns price in gold, to buy amount of resource 
    public Price GetBuyPrice(ResourceName resource, int amount)
    {
        var conf = resourceConfigs.Find(c => c.resource == resource);
        return new Price(
            TransactionType.ResourceMarket,
            new List<Resource>
            {
                new Resource
                {
                    amount = amount * conf.buyPrice,
                    name = ResourceName.Gold
                }
            }
        );
    }

    // Returns price in resource, to sell amount of resource 
    public Price GetSellPrice(ResourceName resource, int amount)
    {
        return new Price(
            TransactionType.ResourceMarket,
            new List<Resource>
            {
                new Resource
                {
                    amount = amount,
                    name = resource
                }
            }
        );
    }

    public void SetTransaction(ResourceName resource, ResourceDynamic dynamic)
    {
        if (!weeklyTransactions.ContainsKey(resource))
            weeklyTransactions.Add(resource, dynamic);
        else
            weeklyTransactions[resource] = dynamic;
    }

    public bool CheckDynamic(ResourceName resource, ResourceDynamic dynamic)
    {
        var conf = GetConfig(resource);

        return (0 <= dynamic.spent && conf.maxSellAmount >= dynamic.gained
            && 0 <= dynamic.spent && conf.maxSellAmount >= dynamic.gained);
    }

    public Dictionary<ResourceName, ResourceDynamic>
        MakeWeeklyTransactions(ResourceContainer reciever)
    {
        var res = new Dictionary<ResourceName, ResourceDynamic>();

        foreach (var (resource, amount) in
            weeklyTransactions.Select(i => (i.Key, i.Value)))
        {
            if (amount.gained != 0 && reciever.CanAfford(GetBuyPrice(resource, amount.gained)))
            {
                reciever.GainResource(resource, amount.gained, TransactionType.ResourceMarket);
            }
            if (amount.spent != 0 && reciever.CanAfford(GetSellPrice(resource, amount.spent)))
            {
                reciever.SpendResource(resource, amount.spent, TransactionType.ResourceMarket);
            }

            if (amount.spent != 0 || amount.gained != 0)
            {
                amount.resource = resource;
                res.Add(resource, amount);
            }
        }

        return res;
    }

    private void OnEnable()
    {
        weeklyTransactions = new();
    }

}