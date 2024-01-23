using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ResourceDynamic
    {
        public ResourceName resource;
        public int gained;
        public int spent;

        public ResourceDynamic()
        {

        }

        public ResourceDynamic(ResourceName resource, int gained, int spent)
        {
            this.resource = resource;
            this.gained = gained;
            this.spent = spent;
        }
    }
}
