﻿using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceContainer", menuName = "ScriptableObjects/ResourceSystem/Container")]
    public class ResourceContainer : ScriptableObject
    {
        [SerializeField]
        private List<Resource> resources;

        private Dictionary<ResourceName, Dictionary<TransactionType, ResourceDynamic>> statistics;

        public List<Production> production;
        public ResourceMarket resourceMarket;

        public List<Resource> Resources => resources;
    
        public Resource GetResource(ResourceName name)
        {
            return resources.Find(r => r.name == name);
        }

        public int GetResourceAmount(ResourceName name) 
        {
            return GetResource(name).amount;
        }

        public IEnumerable<string> 
            GetProductionBuildingsGuids(ResourceName resource)
        {
            return GetActiveProductions(resource)
                .Select(p => p.buildingGuid);
        }

        public IEnumerable<Production> GetActiveProductions(ResourceName resource) 
        {
            return production.Where(p => p.workers > 0 &&
                p.recipe.resources.FindIndex(r => r.name == resource) > 0);
        }

        public Dictionary<TransactionType, ResourceDynamic> 
            GetStatistics(ResourceName resource)
        {
            if (statistics is not null
                && statistics.ContainsKey(resource))
            {
                return statistics[resource];
            }
            else return new();
        }

        public void AddStatistics(ResourceName resource,
            Dictionary<TransactionType, ResourceDynamic> record)
        {
            if (statistics is null)
                statistics = new();

            if (!statistics.ContainsKey(resource))
                statistics[resource] = record;
            else
                throw new Exception("there is already statistic data about: " 
                    + Enum.GetName(typeof(ResourceName), resource)); 
        }

        public void GainResource(ResourceName name, 
            int amount, TransactionType transactionType)
        {
            var gained = GetResource(name).GainResource(amount, transactionType);

            InitDict(name, transactionType);

            statistics[name][transactionType].gained += gained;
            statistics[name][TransactionType.Any].gained += gained;
        }

        public void SpendResource(ResourceName name,
            int amount, TransactionType transactionType)
        {
            var spent = GetResource(name).SpendResource(amount, transactionType);

            InitDict(name, transactionType);

            statistics[name][transactionType].spent += spent;
            statistics[name][TransactionType.Any].spent += spent;
        }

        public void AddResource(Resource resource)
        {
            resources.Add(resource);
        }

        public void AddResources(IEnumerable<Resource> resources)
        {
            this.resources.AddRange(resources);
        }

        public bool CanAfford(Price price)
        {
            foreach (Resource r in price.resources) 
            {
                if (!GetResource(r.name).CanAfford(r.amount,
                    price.transactionType))
                {
                    return false;
                }
            }

            return true;
        }

        public void Spend(Price price)
        {
            if (CanAfford(price))
            {
                foreach (var r in price.resources)
                    SpendResource(r.name, r.amount, price.transactionType);
            }
            else
            {
                Debug.Log("Can't afford!");
            }

        }

        public void Gain(Price price)
        {
            foreach(var r in price.resources)
                GainResource(r.name, r.amount, price.transactionType);
        }

        public int SpentPerWeek(ResourceName resource)
        {
            int value = 0;
            var res = GetResource(resource);
            var releventProductions = production.Where(p => p.workers > 0 &&
                p.recipe.price.resources.FindIndex(r => r.name == resource) > 0);

            foreach (var prod in releventProductions)
            {
                var priceRes = prod.recipe.price
                    .resources.Find(r => r.name == resource);

                if (priceRes is not null) 
                {
                    value += res.ValueWithModifiers(priceRes.amount,
                        TransactionType.Production, false) * prod.workers;
                }
            }

            value += resourceMarket.GetTransaction(resource).spent;

            return value;
        }

        public int GaindedPerWeek(ResourceName resource)
        {
            int value = 0;
            var res = GetResource(resource);

            foreach (var prod in GetActiveProductions(resource))
            {
                var gain = prod.recipe.resources.Find(r => r.name == resource);

                if (gain is not null)
                {
                    value += res.ValueWithModifiers(gain.amount,
                        TransactionType.Production, true) * prod.workers;
                }
            }

            value += resourceMarket.GetTransaction(resource).gained;
            return value;
        }

        private void InitDict(ResourceName resource, TransactionType transaction)
        {
            if (statistics is null)
                statistics = new();

            if (!statistics.ContainsKey(resource))
                statistics.Add(resource, new());

            if (!statistics[resource].ContainsKey(transaction))
                statistics[resource].Add(transaction, 
                    new ResourceDynamic { gained = 0, spent = 0 });

            if (!statistics[resource].ContainsKey(TransactionType.Any))
                statistics[resource].Add(TransactionType.Any,
                    new ResourceDynamic { gained = 0, spent = 0 });
        }

        private void OnEnable()
        {
            statistics = new();
        }
    }
}
