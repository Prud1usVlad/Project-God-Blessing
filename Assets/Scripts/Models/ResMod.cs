using Assets.Scripts.ResourceSystem;
using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResMod
    {
        public ResourceName resource;
        public bool forGain = true;
        public ResourceModifier modifier;
    }
}
