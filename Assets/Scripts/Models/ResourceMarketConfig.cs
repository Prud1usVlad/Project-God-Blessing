using Assets.Scripts.ResourceSystem;
using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResourceMarketConfig
    {
        public ResourceName resource;
        public int buyPrice;
        public int sellPrice;
        public int maxSellAmount;
        public int maxBuyAmount;
    }
}
