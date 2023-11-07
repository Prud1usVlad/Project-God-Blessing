using UnityEngine;

namespace Assets.Scripts.TooltipSystem
{
    public abstract class TooltipDataProvider : MonoBehaviour
    {
        public abstract string GetHeader(string tag = null);
        public abstract string GetContent(string tag = null);
    }
}
