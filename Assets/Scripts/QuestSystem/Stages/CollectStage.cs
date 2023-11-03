using Assets.Scripts.SkillSystem;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public class CollectStage : QuestStage
    {
        // TODO Add corresponding fields and logic
        // when looting system will be finished

        public int toCollect;

        public override float CalcProgress(Dictionary<string, object> arguments)
        {
            var amount = (int)arguments["amount"];

            return  amount / toCollect;
        }
    }

}
