using Assets.Scripts.QuestSystem.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.QuestSystem
{
    public enum QuestStatus
    {
        Available,
        NotAvailable,
        InProgress,
        Completed,
    }

    [Serializable]
    public class Quest
    {
        public QuestData data;
        public string questDataGuid;
        public List<StageInstance> stages;
        public QuestStatus status;
        public int currentStageIdx;

        public float progress => 
            (float)stages.Where(s => s.isCompleted).Count() / (float)data.stages.Count;
        public float stageProgress =>
            stage.progress;
        public StageInstance stage => 
            stages[currentStageIdx];

        public Quest() { }

        public Quest(QuestData data, 
            QuestStatus status = QuestStatus.NotAvailable,
            int stageIdx = 0)
        {
            this.data = data;
            questDataGuid = data.Guid;
            this.status = status;
            currentStageIdx = stageIdx;

            stages = data.stages
                .Select(s => new StageInstance(s))
                .ToList();
        }

        public void AddProgress(Dictionary<string, object> arguments)
        {
            if (IsQuestCompleted())
                return;

            stage.AddProgress(arguments);

            if (stage.isCompleted 
                && !IsQuestCompleted())
            {
                currentStageIdx++;
            }
            else if (IsQuestCompleted())
            {
                status = QuestStatus.Completed;
            }
        }

        public void RollBackStage()
        {
            stage.progress = 0;
        }

        public void RollBackQuest()
        {
            stages = data.stages
                .Select(s => new StageInstance(s))
                .ToList();

            currentStageIdx = 0;
        }

        private bool IsQuestCompleted()
        {
            return currentStageIdx + 1 == stages.Count
                && stage.isCompleted;
        }
    }
}
