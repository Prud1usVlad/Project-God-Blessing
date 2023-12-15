using Assets.Scripts.Helpers.ListView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SkillSystem.Ui
{
    public class SecretListItem : MonoBehaviour, IListItem
    {
        public SecretSkill secret;

        public TextMeshProUGUI secretName;
        public TextMeshProUGUI secretDescription;
        public Image underlay;

        public Color publishedColor;

        public Action Selection { get; set; }

        public void FillItem(object data)
        {
            secret = (SecretSkill)data;

            secretName.SetText(secret.skillName);
            secretDescription.SetText(secret.description);

            if (secret.isPublished)
                underlay.color = publishedColor;
        }

        public bool HasData(object data)
        {
            return data.Equals(secret);
        }

        public void OnSelected()
        {
        }

        public void OnSelecting()
        {
            Selection?.Invoke();
        }
    }
}
