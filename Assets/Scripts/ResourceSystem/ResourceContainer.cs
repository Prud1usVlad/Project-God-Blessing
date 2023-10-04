using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Assets.Scripts.ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceContainer", menuName = "ScriptableObjects/Resources")]
    public class ResourceContainer : ScriptableObject
    {
        [SerializeField]
        private List<Resource> resources;

        private Dictionary<ResourceName, Dictionary<TransactionType, ResourceStatData>> statistics;
        
        public  List<ProductionBuilding> productionBuildings;

        public List<Resource> Resources => resources;
    
        public Resource GetResource(ResourceName name)
        {
            return resources.Find(r => r.name == name);
        }

        public int GetResourceAmount(ResourceName name) 
        {
            return GetResource(name).amount;
        }

        public IEnumerable<ProductionBuilding> 
            GetProductionBuildings(ResourceName resource)
        {
            return productionBuildings.Where(b => b.resource.name == resource);
        }

        public Dictionary<TransactionType, ResourceStatData> 
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
            Dictionary<TransactionType, ResourceStatData> record)
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

        public int SpentPerDay(ResourceName resource)
        {
            int value = 0;
            var res = GetResource(resource);

            foreach (var b in productionBuildings)
            {
                var priceRes = b.productionPrice.resources.Find(r => r.name == resource);

                if (priceRes is not null) 
                {
                    value += res.ValueWithModifiers(priceRes.amount,
                        TransactionType.Production, false) * b.productionPower;
                }
            }

            return value;
        }

        public int GaindedPerDay(ResourceName resource)
        {
            var res = GetResource(resource);

            return productionBuildings
                .Where(b => b.resource.name == resource)
                .Sum(b => res.ValueWithModifiers(
                    b.GetProductionAmount(), 
                    TransactionType.Production, 
                    true)
                );
        }

        private void InitDict(ResourceName resource, TransactionType transaction)
        {
            if (statistics is null)
                statistics = new();

            if (!statistics.ContainsKey(resource))
                statistics.Add(resource, new());

            if (!statistics[resource].ContainsKey(transaction))
                statistics[resource].Add(transaction, 
                    new ResourceStatData { gained = 0, spent = 0 });

            if (!statistics[resource].ContainsKey(TransactionType.Any))
                statistics[resource].Add(TransactionType.Any,
                    new ResourceStatData { gained = 0, spent = 0 });
        }

        private void OnEnable()
        {
            statistics = new();
            productionBuildings = new();
        }
    }
}
