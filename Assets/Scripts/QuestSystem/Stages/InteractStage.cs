using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public class InteractStage : QuestStage
    {
        // TODO Add corresponding fields and logic
        // when looting system will be finished

        public override float CalcProgress(Dictionary<string, object> arguments)
        {
            return 1;
        }
    }
}
