using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Ui.Widgets
{
    public class ResourceLogWidget : MonoBehaviour, IListItem
    {
        public ResourceDynamic rDynamic;

        public TextMeshProUGUI amount;
        public TextMeshProUGUI gained;
        public TextMeshProUGUI used;
        public Image icon;

        public ResourceContainer resourceContainer;
        public ResourceDescriptions resourceDescriptions;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            rDynamic = (ResourceDynamic)data;
            amount.SetText(Converters.IntToUiString(resourceContainer.GetResourceAmount(rDynamic.resource)));

            gained.SetText(Converters.IntToUiString(rDynamic.gained));
            used.SetText(Converters.IntToUiString(rDynamic.spent));

            icon.sprite = resourceDescriptions.GetResourceIcon(rDynamic.resource);
        }

        public bool HasData(object data)
        {
            if (data is ResourceDynamic)
            {
                return (data as ResourceDynamic) == rDynamic;
            }
            else return false;
        }

        public void OnSelected()
        {

        }

        public void OnSelecting()
        {
        }

    }
}
