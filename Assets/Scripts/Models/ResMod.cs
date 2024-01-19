using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ResourceSystem;
using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResMod
    {
        public ResourceName resource;
        public ModifierReciever reciever;
        public bool forGain = true;
        public ResourceModifier modifier;
    }
}
