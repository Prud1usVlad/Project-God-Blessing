using Assets.Scripts.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public abstract class QuestStage
    {
        public string shortDescription;
        [TextArea]
        public string longDescription;

        public abstract float CalcProgress(Dictionary<string, object> arguments);
    }
}
