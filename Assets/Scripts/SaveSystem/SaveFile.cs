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
        public List<NationLearnedSkills> learnedSkills;

        // Hub config
        public List<Place> places;
        public List<Resource> resources;
        public List<Curse> curses;
        public List<ProductionPower> productionPowers;
        public List<ResourceDynamic> everydayTransactions;


        public void ReadFromGameProgress(GameProgress progress)
        {
            SaveBasic(progress);

            SaveCurrentCurses(progress);

            SaveResources(progress);
            
            SaveSkills(progress);
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            LoadBasic(progress);

            LoadResources(progress);

            LoadCurses(progress);

            LoadModifiers(progress);

            LoadProduction(progress);
            
            LoadSkills(progress);

        }

        private void LoadSkills(GameProgress progress)
        {
            foreach (var nation in learnedSkills)
            {
                var reg = progress.skillSystem.skillRegistries
                    .Find(r => r.nation == nation.nation);

                foreach (var skill in nation.skillGuids)
                {
                    reg.FindByGuid(skill).isLearnd = true;
                }
            }
        }

        private void LoadProduction(GameProgress progress)
        {
            foreach (var b in progress.resourceContainer.productionBuildings)
            {
                var power = productionPowers.Find(p => p.buildingGuid == b.Guid);
                if (power is not null)
                {
                    b.productionPower = power.power;
                }
            }
        }

        private void LoadModifiers(GameProgress progress)
        {
            foreach (var c in progress.curses)
                progress.globalModifiers.AddModifiers(c);

            foreach (var b in progress.placedBuildings)
                if (b is BonusBuilding)
                    progress.globalModifiers.AddModifiers((BonusBuilding)b);
        }

        private void LoadCurses(GameProgress progress)
        {
            curses.ForEach(c => progress.curses.Add(progress.curseRegistry
                            .InitByGuid(c.guid, c.prophesyIdx, c.imageIdx)));
        }

        private void LoadResources(GameProgress progress)
        {
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
        }

        private void LoadBasic(GameProgress progress)
        {
            progress.fameTranslation.SetPoints(fame);
            progress.liesTranslation.SetPoints(lies);
        }

        private void SaveResources(GameProgress progress)
        {
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

        private void SaveCurrentCurses(GameProgress progress)
        {
            curses = progress.curses.Select(c =>
                            new Curse
                            {
                                guid = c.Guid,
                                imageIdx = c.imageIdx,
                                prophesyIdx = c.prophesyIdx
                            }
                        ).ToList();
        }

        private void SaveBasic(GameProgress progress)
        {
            day = progress.day;
            fame = progress.fameTranslation.currentPoints;
            lies = progress.liesTranslation.currentPoints;
            reserchedBuildings = progress.buildingResearch;
        }

        private void SaveSkills(GameProgress progress)
        {
            learnedSkills = new List<NationLearnedSkills>();
            foreach (var registry in progress.skillSystem.skillRegistries)
            {
                learnedSkills.Add(new NationLearnedSkills
                {
                    nation = registry.nation,
                    skillGuids = registry.skills
                        .Where(s => s.isLearnd)
                        .Select(s => s.Guid)
                        .ToList()
                });
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

        [Serializable]
        public class NationLearnedSkills
        {
            public NationName nation;
            public List<string> skillGuids;
        }
    }
}
