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
        var prodBuild = objectPlacer.placedGameObjects
            .Select(o => o.GetComponent<BuildingController>().building)
            .Where(b => b is ProductionBuilding)
            .Cast<ProductionBuilding>();
        var cont = progress.resourceContainer;

        foreach (var b in prodBuild)
        {
            if (b.productionPower < 0)
                continue;

            var totalPrice = b.productionPrice * b.productionPower;

            if (cont.CanAfford(totalPrice))
            {
                cont.Spend(totalPrice);

                var totalAmount = Mathf.RoundToInt(b.resource.amount
                    * b.productionMultiplier * b.productionPower);

                cont.GainResource(b.resource.name, totalAmount, TransactionType.Production);
            }
            else
            {
                Debug.Log("Production stopped in " + b.buildingName);
            }
        }
    }
}
