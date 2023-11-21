using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Stats;

namespace Assets.Scripts.MainMenu.CharacterCreation
{
    public class StatSelector : MonoBehaviour
    {
        public static int freePoints;
        [NonSerialized]
        public int prevFreePoints;

        public StatName statName;
        public int value = 0;
        public bool allowInteractions;

        public TextMeshProUGUI statNameField;
        public List<Button> buttons;
        public Color activeColor;
        public Color defaultColor;

        public void Awake()
        {
            prevFreePoints = freePoints;
            statNameField.SetText(Enum.GetName(typeof(StatName), statName));

            UpdateView();
        }

        public void SetValue(int newValue) 
        {
            if (!allowInteractions)
                return;

            UpdateValue(newValue);

            UpdateView();
        }

        public void UpdateView(int newValue = -1)
        {
            if (newValue >= 0)
                UpdateValue(newValue, false, false);

            foreach (var (button, i) in buttons.Select((b, i) => (b, i))) 
            {
                if (i < value) 
                {
                    button.image.color = activeColor;
                    button.enabled = true;
                }
                else if (i - value < freePoints) 
                {
                    button.image.color = defaultColor;
                    button.enabled = true;
                }
                else
                {
                    button.image.color = defaultColor;
                    button.enabled = false;
                }
            }
        }

        private void Update()
        {
            if (prevFreePoints != freePoints)
                UpdateView();
        }

        private void UpdateValue(int newValue, 
            bool sameValueNullifies = true, bool withFreePoints = true)
        {
            if (sameValueNullifies && value == newValue)
            {
                if (withFreePoints)
                    freePoints += value;
                value = 0;
            }
            else
            {
                if (withFreePoints)
                    freePoints += value - newValue;
                value = newValue;
            }
        }
    }
}
