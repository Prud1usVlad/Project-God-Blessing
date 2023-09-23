using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ResourceSystem
{
    [Serializable]
    public class Price
    {
        public TransactionType transactionType;
        public List<Resource> resources;


    }
}
