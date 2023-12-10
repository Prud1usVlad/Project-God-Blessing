using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.ListView;
using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Hub.Buildings.Ui
{
    public class ProductionWidget : MonoBehaviour, IListItem
    {
        private IWorkersChecker workersChecker;

        public Production production;

        public TextMeshProUGUI recipeName;
        public Slider slider;
        public ListViewController prodResources;
        public ListViewController consResources;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            production = data as Production;
            workersChecker = GetComponentInParent<IWorkersChecker>();

            recipeName.SetText(production.recipe.name);
            slider.value = production.workers;

            UpdateResources();
        }

        private void UpdateResources()
        {
            prodResources.InitView(production.recipe.resources
                .Select(r => new Resource(r) { amount = r.amount * production.workers })
                .Cast<object>().ToList());

            consResources.InitView(production.recipe.price.resources
                .Select(r => new Resource(r) { amount = r.amount * production.workers })
                .Cast<object>().ToList());
        }

        public void OnSliderValueChanged()
        {
            var newVal = (int)slider.value;

            if (production.workers != newVal)
            {
                if (workersChecker.ChekWorkers(production.recipe.name, newVal))
                {
                    production.workers = newVal;
                    workersChecker.UpdateWorkers();
                    UpdateResources();
                }
                else
                {
                    slider.value = production.workers;
                }
            }
        } 

        public bool HasData(object data)
        {
            return production.Equals(data);
        }

        public void OnSelected()
        {

        }

        public void OnSelecting()
        {

        }
    }
}
