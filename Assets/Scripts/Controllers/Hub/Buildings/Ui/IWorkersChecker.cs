using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Controllers.Hub.Buildings.Ui
{
    public interface IWorkersChecker
    {
        public bool ChekWorkers(string recipeName, int newValue);
        public void UpdateWorkers(IEnumerable<Production> production = null);
    }
}
