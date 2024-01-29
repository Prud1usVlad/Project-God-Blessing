using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Production
    {
        public ProductionRecipe recipe;
        public string buildingGuid;
        public int workers;

        public Production(ProductionRecipe recipe, string buildingGuid, int workers = 0)
        {
            this.recipe = recipe;
            this.buildingGuid = buildingGuid;
            this.workers = workers;
        }

    }
}
