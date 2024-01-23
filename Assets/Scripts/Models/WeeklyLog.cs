using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class WeeklyLog
    {
        public int week = 0;
        public Dictionary<ResourceName, ResourceDynamic> resourceMarketLog;
        public List<BuildingProductionLogItem> productionBuldingsLog;
        public List<Building> productionStoppedLog;

        public WeeklyLog() 
        {
            resourceMarketLog = new Dictionary<ResourceName, ResourceDynamic>();
            productionBuldingsLog = new List<BuildingProductionLogItem>();
            productionStoppedLog = new List<Building>();
        }

        public WeeklyLog(int week, Dictionary<ResourceName, ResourceDynamic> resourceMarketLog, 
            List<BuildingProductionLogItem> productionBuldingsLog, 
            List<Building> productionStoppedBuildingGuids)
        {
            this.week = week;
            this.resourceMarketLog = resourceMarketLog;
            this.productionBuldingsLog = productionBuldingsLog;
            this.productionStoppedLog = productionStoppedBuildingGuids;
        }

        public WeeklyLog(int week) : this()
        {
            this.week = week;
        }
    }
}
