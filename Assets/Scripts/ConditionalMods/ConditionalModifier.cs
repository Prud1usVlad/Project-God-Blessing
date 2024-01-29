using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.StatSystem
{
    public abstract class ConditionalModifier : SerializableScriptableObject
    {
        public GlobalModifiers modifiers;
        public GameProgress gameProgress;

        public Object source;
        public bool isAdded = false;

        public void ManageModifier()
        {
            if (CheckCondition())
            {
                AddModifiers();
                isAdded = true;
            }
            else if (isAdded)
            {
                DeleteModifiers();
                isAdded = false;
            }
        }

        protected abstract bool CheckCondition();
        protected abstract void AddModifiers();
        protected abstract void DeleteModifiers();
    }
}
