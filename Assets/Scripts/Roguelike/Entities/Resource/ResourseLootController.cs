using System.Collections.Generic;
using Assets.Scripts.Helpers.Roguelike;
using UnityEngine;

public class ResourceLootController : MonoBehaviour
{
    public List<AvailableResource> AvailableResources;

    private LootHandler _lootHandler;

    private void Start()
    {
        _lootHandler = LootHandler.Instance;
    }

    public void Loot()
    {
        foreach (AvailableResource resource in AvailableResources)
        {
            if (RandomHelper.GetResultOfChanceWheel(resource.LootChanse))
            {
                _lootHandler.CollectResource(new CollectedResourceData
                {
                    Name = resource.Name,
                    LootedAmount = Random.Range(resource.MinAmount, resource.MaxAmount)
                });
            }
        }
    }
}