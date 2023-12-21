using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.QuestSystem.Stages
{
    [Serializable]
    public class StageInstance
    {
        public QuestStage data;

        public string shortDescription => data?.shortDescription;
        public string longDescription => data?.longDescription;
        public float progress = 0;
        public bool isCompleted = false;

        public void AddProgress(Dictionary<string, object> arguments)
        {
            progress += data.CalcProgress(arguments);
            progress = Mathf.Min(progress, 1);

            if (progress == 1)
                isCompleted = true; 
        }

        public StageInstance() { }

        public StageInstance(QuestStage data) 
        {
            this.data = data;
        }
    }
}
