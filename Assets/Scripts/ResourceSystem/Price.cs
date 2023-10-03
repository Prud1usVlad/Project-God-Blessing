using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ResourceSystem
{
    [Serializable]
    public class Price
    {
        public TransactionType transactionType;
        public List<Resource> resources;

        public static Price operator +(Price a, int amount)
        {
            var newP = new Price(a);

            newP.resources.ForEach(r => r.amount += amount); 

            return newP;
        }

        public static Price operator *(Price a, float amount)
        {
            var newP = new Price(a);

            newP.resources.ForEach(r => 
                r.amount = Mathf.RoundToInt(r.amount * amount));

            return newP;
        }

        public Price(Price other)
        {
            transactionType = other.transactionType;
            resources = new List<Resource>();

            other.resources.ForEach(r => 
                resources.Add(new Resource() 
                    { 
                        amount = r.amount, 
                        name = r.name
                    }
                )
            );
        }
    }
}
