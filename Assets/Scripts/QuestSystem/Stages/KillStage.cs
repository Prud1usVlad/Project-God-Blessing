using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public class KillStage : QuestStage
    {
        public int toKill;

        // TODO Add corresponding fields and logic
        // when fighting system will be finished

        public override float CalcProgress(Dictionary<string, object> arguments)
        {
            var amount = (int)arguments["amount"];
            return amount / toKill;
        }
    }
}
