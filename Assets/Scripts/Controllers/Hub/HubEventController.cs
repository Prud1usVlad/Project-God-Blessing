using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.Models;
using System.Security.Cryptography;
using Assets.Scripts.ScriptableObjects.Hub;
using Assets.Scripts.Controllers.Ui.DialogueBoxes;

public class HubEventController : MonoBehaviour
{
    public GameProgress progress;
    public ModalManager modalManager;

    public WeeklyLogWindow weeklyLogWindow;
    public Transform canvas;

    public WeeklyLog OnNextWeek()
    {
        progress.week += 1;

        var log = new WeeklyLog(progress.week);

        ProduceResources(log);
        MarketTransactions(log);

        return log;
    }

    private void ProduceResources(WeeklyLog log)
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
                var record = new Dictionary<ResourceName, ResourceDynamic>();
                resources.Spend(totalPrice);

                foreach (var resource in totalPrice.resources)
                    record[resource.name] = new ResourceDynamic 
                        { gained = 0, spent = resource.amount, resource = resource.name };

                foreach (var res in item.recipe.resources)
                {
                    if (!record.ContainsKey(res.name))
                        record[res.name] = new ResourceDynamic 
                            { gained = 0, spent = 0, resource = res.name };

                    var totalAmount = item.recipe.TotalAmount(res.name, item.workers);
                    resources.GainResource(res.name, totalAmount, TransactionType.Production);

                    record[res.name].gained += totalAmount;
                }

                log.productionBuldingsLog.Add(new BuildingProductionLogItem
                {
                    building = progress.placedBuildings.First(b => b.Guid == item.buildingGuid),
                    resources = record.Values.ToList(),
                });
            }
            else
            {
                Debug.Log("Production stopped in " + item.buildingGuid);
                log.productionStoppedLog
                    .Add(progress.placedBuildings.First(b => b.Guid == item.buildingGuid));
            }
        }
    }

    private void MarketTransactions(WeeklyLog log)
    {
        var container = progress.resourceContainer;
        
        log.resourceMarketLog = container
            .resourceMarket.MakeWeeklyTransactions(container);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab)) 
        {
            Debug.Log("Next week");
            var log = OnNextWeek();

            var modal = Instantiate(weeklyLogWindow, canvas);

            modal.log = log;

            modalManager.DialogueOpen(modal);

        }
    }
}
