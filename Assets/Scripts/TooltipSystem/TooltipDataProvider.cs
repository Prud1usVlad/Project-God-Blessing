using UnityEngine;

namespace Assets.Scripts.TooltipSystem
{
    public abstract class TooltipDataProvider : MonoBehaviour
    {
        public abstract string GetHeader();
        public abstract string GetContent();
    }
}
