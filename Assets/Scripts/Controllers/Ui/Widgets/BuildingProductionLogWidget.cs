using Assets.Scripts.Helpers.ListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;

namespace Assets.Scripts.Controllers.Ui.Widgets
{
    public class BuildingProductionLogWidget : MonoBehaviour, IListItem
    {
        private BuildingProductionLogItem logItem;

        public TextMeshProUGUI buildingName;

        public ListViewController consumptionList;
        public ListViewController productionList;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            logItem = (BuildingProductionLogItem)data;

            buildingName.SetText(logItem.building.buildingName);

            consumptionList.InitView(logItem.resources
                .Where(d => d.spent > 0)
                .Select(d => new Resource() { name = d.resource, amount = d.spent })
                .Cast<object>().ToList());

            productionList.InitView(logItem.resources
                .Where(d => d.gained > 0)
                .Select(d => new Resource() { name = d.resource, amount = d.gained })
                .Cast<object>().ToList());
        }

        public bool HasData(object data)
        {
            return data != null ? data.Equals(logItem) : false;
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
        }
    }
}
