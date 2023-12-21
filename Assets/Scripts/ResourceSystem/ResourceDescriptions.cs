using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ResourceSystem
{
    [CreateAssetMenu(fileName = "ResourceDescriptions", menuName = "ScriptableObjects/ResourceSystem/Descriptions")]
    public class ResourceDescriptions : ScriptableObject
    {
        [SerializeField]
        private List<ListEntry> data;

        public string GetResourceDescription(ResourceName name)
        {
            return data.Find(i => i.resource == name).description;
        }

        public string GetResourceHumanName(ResourceName name)
        {
            return data.Find(i => i.resource == name).humanName;
        }

        public Sprite GetResourceIcon(ResourceName name)
        {
            return data.Find(i => i.resource == name).icon;
        }


        [Serializable]
        private class ListEntry
        {
            public ResourceName resource;
            public string humanName;
            [TextArea]
            public string description;
            public Sprite icon;
        }
    }
}
