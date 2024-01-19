using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.ResourceSystem;

public class HubEventController : MonoBehaviour
{
    public GameProgress progress;

    public ObjectPlacer objectPlacer;

    public void OnNextDay()
    {
        progress.day += 1;

        ProduceResources();
    }

    private void ProduceResources()
    {
        var production = progress.production;
        var resources = progress.resourceContainer;

        foreach (var item in production)
        {
            if (item.workers < 0)
                continue;

            var totalPrice = item.recipe.price * item.workers;

            if (resources.CanAfford(totalPrice))
            {
                resources.Spend(totalPrice);

                foreach (var res in item.recipe.resources)
                {
                    var totalAmount = item.recipe.TotalAmount(res.name, item.workers);
                    resources.GainResource(res.name, totalAmount, TransactionType.Production);
                }
            }
            else
            {
                Debug.Log("Production stopped in " + item.buildingGuid);
            }
        }
    }
}
