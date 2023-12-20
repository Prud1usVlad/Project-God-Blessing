using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ProductionRecipe
    {
        public string name;
        public List<Resource> resources;
        public Price price;
    }
}
