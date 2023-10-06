using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts.SaveSystem
{
    [Serializable]
    public class SaveFile
    {
        // Header
        public string characterName;
        public string type;
        public int fame;
        public int lies;
        public int day;
        public string date;

        // Progress
        public List<ItemAvaliability> reserchedBuildings;
        public List<ResStatItem> resourceStatistics;

        // Hub config
        public List<Place> places;
        public List<Resource> resources;
        public List<Curse> curses;
        public List<ProductionPower> productionPowers;
        public List<ResourceDynamic> everydayTransactions;


        public void ReadFromGameProgress(GameProgress progress)
        {
            // save basic data
            day = progress.day;
            fame = progress.fameTranslation.currentPoints;
            lies = progress.liesTranslation.currentPoints;
            reserchedBuildings = progress.buildingResearch;

            // save current curses
            curses = progress.curses.Select(c =>
                new Curse
                {
                    guid = c.Guid,
                    imageIdx = c.imageIdx,
                    prophesyIdx = c.prophesyIdx
                }
            ).ToList();

            // save resources and their statistics
            resources = progress.resourceContainer.Resources;
            resourceStatistics = new List<ResStatItem>();
            everydayTransactions = new();

            foreach (var res in progress.resourceContainer.Resources)
            {
                var transaction = progress.resourceContainer
                    .resourceMarket.GetTransaction(res.name);
                transaction.resource = res.name;
                everydayTransactions.Add(transaction);

                foreach (var item in progress.resourceContainer.GetStatistics(res.name))
                {
                    resourceStatistics.Add(new ResStatItem
                    {
                        resource = res.name,
                        transactionType = item.Key,
                        gained = item.Value.gained,
                        spent = item.Value.spent,
                    });
                }
            }

            productionPowers = progress.resourceContainer.productionBuildings
                .Select(b => new ProductionPower
                {
                    buildingGuid = b.Guid,
                    power = b.productionPower
                }).ToList();
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            // set basic values
            progress.fameTranslation.SetPoints(fame);
            progress.liesTranslation.SetPoints(lies);

            // init all reserched buildings
            foreach (var b in reserchedBuildings)
            {
                var item = progress.buildingResearch
                    .Find(o => b.guid == o.guid);

                item.isAvaliable = b.isAvaliable;
            }

            // set resources
            progress.resourceContainer.Resources.Clear();
            progress.resourceContainer.AddResources(resources);
            var resGroup = resourceStatistics.GroupBy(r => r.resource);

            foreach (var res in everydayTransactions)
            {
                progress.resourceContainer.resourceMarket
                    .SetTransaction(res.resource, res);
            }

            foreach (var group in resGroup)
            {
                var record = new Dictionary<TransactionType, ResourceDynamic>();

                foreach (var item in group)
                {
                    record.Add(item.transactionType,
                        new ResourceDynamic
                        {
                            gained = item.gained,
                            spent = item.spent
                        }
                    );
                }

                progress.resourceContainer.AddStatistics(group.Key, record);
            }

            // set curses
            curses.ForEach(c => progress.curses.Add(progress.curseRegistry
                .InitByGuid(c.guid, c.prophesyIdx, c.imageIdx)));

            // set modifiers
            foreach (var c in progress.curses)
                progress.globalModifiers.AddModifiers(c);

            foreach (var b in progress.placedBuildings)
                if (b is BonusBuilding)
                    progress.globalModifiers.AddModifiers((BonusBuilding)b);

            // set production powers
            foreach (var b in progress.resourceContainer.productionBuildings)
            {
                var power = productionPowers.Find(p => p.buildingGuid == b.Guid);
                if (power is not null)
                {
                    b.productionPower = power.power;
                }
            }

        }


        // models used only in serialization
        [Serializable]
        public class Place
        {
            public Vector3 position;
            public string buildingGuid;
        }

        [Serializable]
        public class Curse
        {
            public string guid;
            public int imageIdx;
            public int prophesyIdx;
        }

        [Serializable]
        public class ResStatItem
        {
            public ResourceName resource;
            public TransactionType transactionType;
            public int gained;
            public int spent;
        }

        [Serializable]
        public class ProductionPower
        {
            public string buildingGuid;
            public int power;
        }
    }
}
