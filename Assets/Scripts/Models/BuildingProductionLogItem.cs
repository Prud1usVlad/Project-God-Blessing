using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class BuildingProductionLogItem
    {
        public Building building;
        public List<ResourceDynamic> resources;
    }
}
