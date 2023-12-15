using Assets.Scripts.ResourceSystem;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Models;

[CreateAssetMenu(fileName = "ResourceMarket", menuName = "ScriptableObjects/ResourceSystem/Market")]
public class ResourceMarket : ScriptableObject
{
    private Dictionary<ResourceName, ResourceDynamic> everydayTransactions;

    public List<ResourceMarketConfig> resourceConfigs;

    public ResourceMarketConfig GetConfig(ResourceName resource)
    {
        return resourceConfigs.Find(c => c.resource == resource);
    }

    public ResourceDynamic GetTransaction(ResourceName resource)
    {
        if (everydayTransactions.ContainsKey(resource))
            return everydayTransactions[resource];
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
        if (!everydayTransactions.ContainsKey(resource))
            everydayTransactions.Add(resource, dynamic);
        else
            everydayTransactions[resource] = dynamic;
    }

    public bool CheckDynamic(ResourceName resource, ResourceDynamic dynamic)
    {
        var conf = GetConfig(resource);

        return (0 <= dynamic.spent && conf.maxSellAmount >= dynamic.gained
            && 0 <= dynamic.spent && conf.maxSellAmount >= dynamic.gained);
    }

    private void OnEnable()
    {
        everydayTransactions = new();
    }

}