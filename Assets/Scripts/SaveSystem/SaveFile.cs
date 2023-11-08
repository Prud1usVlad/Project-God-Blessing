using Assets.Scripts.EquipmentSystem;
using Assets.Scripts.Models;
using Assets.Scripts.QuestSystem;
using Assets.Scripts.QuestSystem.Stages;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
        public List<ConnectionData> connectionsData;
        public List<NationLearnedSkills> learnedSkills;
        public List<string> equipedSkills;
        public List<InventoryRecord> inventoryRecords;
        public List<Quest> completedQuests;
        public List<Quest> availableQuests;

        // Hub config
        public List<Place> places;
        public List<Resource> resources;
        public List<Curse> curses;
        public List<ProductionPower> productionPowers;
        public List<MarketBuildingData> marketBuildingsData;
        public List<ResourceDynamic> everydayTransactions;


        public void ReadFromGameProgress(GameProgress progress)
        {
            SaveBasic(progress);
            SaveCurrentCurses(progress);
            SaveResources(progress);
            SaveBuildingStates(progress);
            SaveSkills(progress);
            SaveInventory(progress);
            SaveQuests(progress);
        }

        public void WriteToGameProgress(GameProgress progress)
        {
            LoadBasic(progress);
            LoadResources(progress);
            LoadCurses(progress);
            LoadModifiers(progress);
            LoadBuildingStates(progress);
            LoadSkills(progress);
            LoadInventory(progress);
            LoadQuests(progress);
        }

        # region Load Methods 

        private void LoadSkills(GameProgress progress)
        {
            progress.skillSystem.learnedSkills = new();
            foreach (var nation in learnedSkills)
            {
                var reg = progress.skillSystem.skillRegistries
                    .Find(r => r.nation == nation.nation);

                foreach (var guid in nation.skillGuids)
                {
                    var skill = reg.FindByGuid(guid);
                    skill.isLearnd = true;
                    progress.skillSystem.learnedSkills.Add(skill);
                }
            }

            foreach (var (guid, i) in equipedSkills.Select((g, i) => (g, i)))
            {
                var skill = progress.skillSystem
                    .learnedSkills.Find(s => s.Guid == guid);

                if (skill.type == SkillType.Active)
                    progress.skillSystem.equipedActiveSkills[i] = skill as ActiveSkill;
                else if (skill.type == SkillType.Value)
                    progress.skillSystem.equipedValueSkill = skill as ValueSkill;
            }

            foreach (var trans in progress.skillSystem.connections)
            {
                if (connectionsData.Count() == 0)
                    break;

                var data = connectionsData
                    .First(d => d.nation == trans.nation);

                trans.SetPoints(data.points);
                trans.freeResearchPoints = data.freeResearchPoints;
                trans.aquiredResearchPoints = data.aquiredResearchPoints;
            }
        }

        private void LoadBuildingStates(GameProgress progress)
        {
            foreach (var b in progress.resourceContainer.productionBuildings)
            {
                var power = productionPowers.Find(p => p.buildingGuid == b.Guid);
                if (power is not null)
                {
                    b.productionPower = power.power;
                }
            }

            foreach (var marketData in marketBuildingsData)
            {
                var building = progress.placedBuildings
                    .Find(b => b.Guid == marketData.buildingGuid);

                (building as MarketBuilding)
                    .InitStore(marketData.itemGuids, marketData.daysTillUpdate); 
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

                item.isAvailable = b.isAvailable;
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

        private void LoadInventory(GameProgress progress)
        {
            foreach(var record in inventoryRecords)
            {
                progress.inventory.Add(record);

                if (record.isEquipped)
                    progress.equipment.Equip(record, false);
            }
        }

        private void LoadQuests(GameProgress progress)
        {
            availableQuests.ForEach(q => InitQuestBeforeLoad(q, progress));
            completedQuests.ForEach(q => InitQuestBeforeLoad(q, progress));

            progress.questSystem.availableQuests = availableQuests;
            progress.questSystem.completedQuests = completedQuests;
        }


        #endregion

        #region Load auxilary methods
        private void InitQuestBeforeLoad(Quest quest, GameProgress progress)
        {
            quest.data = progress.questSystem
                .questRegistry.FindByGuid(quest.questDataGuid);

            for (int i = 0; i < quest.data.stages.Count(); i++)
            {
                quest.stages[i].data = quest.data.stages[i];
            }
        }
        #endregion

        # region Save Methods

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
        }

        private void SaveBuildingStates(GameProgress progress)
        {
            productionPowers = progress.resourceContainer.productionBuildings
                .Select(b => new ProductionPower
                {
                    buildingGuid = b.Guid,
                    power = b.productionPower
                }).ToList();

            var markets = progress.placedBuildings
                .Where(b => b is MarketBuilding)
                .Cast<MarketBuilding>();

            marketBuildingsData = new List<MarketBuildingData>();
            foreach (var market in markets)
            {
                marketBuildingsData.Add(new()
                {
                    buildingGuid = market.Guid,
                    daysTillUpdate = market.daysTillUpdate,
                    itemGuids = market.items
                        .Where(i => i is not null)
                        .Select(i => i.Guid)
                        .ToList(),
                });
            }
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
            learnedSkills = new();
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

            equipedSkills = new(); 

            equipedSkills.AddRange(progress
                .skillSystem.equipedActiveSkills
                .Where(s => s is not null)
                .Select(s => s.Guid));
            if (progress.skillSystem.equipedValueSkill is not null)
                equipedSkills.Add(progress.skillSystem.equipedValueSkill.Guid);

            connectionsData = new();
            foreach (var trans in progress.skillSystem.connections)
            {
                connectionsData.Add(new()
                {
                    nation = trans.nation,
                    points = trans.currentPoints,
                    aquiredResearchPoints = trans.aquiredResearchPoints,
                    freeResearchPoints = trans.freeResearchPoints,
                });
            }
        }

        private void SaveInventory(GameProgress progress)
        {
            inventoryRecords = progress.inventory.records;
        }

        private void SaveQuests(GameProgress progress)
        {
            availableQuests = progress.questSystem.availableQuests;
            completedQuests = progress.questSystem.completedQuests;
        }

        #endregion

        #region Models 
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

        [Serializable]
        public class ConnectionData
        {
            public NationName nation;
            public int points;
            public int freeResearchPoints;
            public int aquiredResearchPoints;
        }

        [Serializable]
        public class MarketBuildingData
        {
            public string buildingGuid;
            public int daysTillUpdate;
            public List<string> itemGuids;
        }

        #endregion
    }
}
