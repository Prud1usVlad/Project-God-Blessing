using System;
using System.Collections.Generic;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public class TravelStage : QuestStage
    {
        // TODO Add corresponding fields and logic
        // when looting system will be finished

        public override float CalcProgress(Dictionary<string, object> arguments)
        {
            return 1;
        }
    }
}
