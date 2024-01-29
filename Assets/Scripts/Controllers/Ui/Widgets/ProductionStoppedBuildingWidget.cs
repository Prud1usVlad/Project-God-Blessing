using Assets.Scripts.Helpers.ListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers.Ui.Widgets
{
    public class ProductionStoppedBuildingWidget : MonoBehaviour, IListItem
    {
        public TextMeshProUGUI buildingName;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            buildingName.SetText(data.ToString());
        }

        public bool HasData(object data)
        {
            return data.ToString() == buildingName.text;
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
        }
    }
}
