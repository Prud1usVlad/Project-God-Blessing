using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ProductionRecipe
    {
        public string name;
        public List<Resource> resources;
        public Price price;

        public int TotalAmount(ResourceName resource, int workers)
        {
            var res = resources.Find(r => r.name == resource);

            return Mathf.RoundToInt(res.ValueWithModifiers(res.amount, TransactionType.Production, true) * workers);
        }
    }
}
