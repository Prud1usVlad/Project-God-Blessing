using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Controllers.Ui.DialogueBoxes
{
    public class WeeklyLogWindow : DialogueBox
    {
        public GameProgress progress;
        public WeeklyLog log;

        public TextMeshProUGUI week;

        public GameObject marketSection;
        public GameObject productionSection;
        public GameObject productionStoppedSection;

        public TextMeshProUGUI message;

        public override bool InitDialogue()
        {
            var inited = base.InitDialogue();

            if (inited)
            {
                UpdateView();
            }

            return inited;
        }

        public void OnClose()
        {
            modalManager.DialogueClose();
        }

        private void UpdateView()
        {
            var hasMarketSection = log.resourceMarketLog.Count > 0;
            var hasProductionSection = log.productionBuldingsLog.Count > 0;
            var hasProductionStoppedSection = log.productionStoppedLog.Count > 0;

            week.SetText(progress.week.ToString());
            
            if (hasMarketSection) { }
                marketSection.GetComponentInChildren<ListViewController>()
                    .InitView(log.resourceMarketLog.Values.Cast<object>().ToList());

            if (hasProductionSection) 
                productionSection.GetComponentInChildren<ListViewController>()
                    .InitView(log.productionBuldingsLog.Cast<object>().ToList());

            if (hasProductionStoppedSection)
                productionStoppedSection.GetComponentInChildren<ListViewController>()
                    .InitView(log.productionStoppedLog.Cast<object>().ToList());

            if (!hasProductionStoppedSection && !hasProductionSection && !hasMarketSection)
                message.gameObject.SetActive(false);
            else 
                message.gameObject.SetActive(true);
        }
    }
}
