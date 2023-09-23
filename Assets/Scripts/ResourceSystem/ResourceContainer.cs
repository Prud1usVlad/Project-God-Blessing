using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceContainer", menuName = "ScriptableObjects/Resources")]
    public class ResourceContainer : ScriptableObject
    {
        [SerializeField]
        private List<Resource> resources;

        public List<Resource> Resources => resources;
    
        public Resource GetResource(ResourceName name)
        {
            return resources.Find(r => r.name == name);
        }

        public int GetResourceAmount(ResourceName name) 
        {
            return GetResource(name).amount;
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
                    GetResource(r.name).SpendResource(r.amount,
                        price.transactionType);
            }
            else
            {
                Debug.Log("Can't afford!");
            }

        }
    }
}
