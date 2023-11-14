using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Assets.Scripts.ResourceSystem;

namespace Assets.Scripts.QuestSystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/QuestSystem/System")]
    public class QuestSystem : ScriptableObject
    {
        public GameProgress gameProgress;

        public QuestRegistry questRegistry;

        public List<Quest> availableQuests;
        public List<Quest> completedQuests;

        public int replayableAmount = 3;

        public List<Quest> activeQuests => availableQuests
            .Where(q => q.status == QuestStatus.InProgress)
            .ToList();

        public void FillAvailable()
        {
            var nations = Enum.GetValues(typeof(NationName));
            FillStoryQuests(nations);
            FillReplayable(nations);
        }

        public void AddProgress(Dictionary<string, object> arguments)
        {
            foreach(var quest in activeQuests)
            {
                quest.AddProgress(arguments);
            }
        }

        public void AcceptQuest(Quest quest)
        {
            quest.status = QuestStatus.InProgress;
        }

        public void AcceptQuest(string guid)
        {
            AcceptQuest(GetQuestByGuid(guid));
        }

        public void DeclineQuest(Quest quest)
        {
            quest.status = QuestStatus.NotAvailable;

            availableQuests.Remove(quest);
        }

        public void DeclineQuest(string guid)
        {
            DeclineQuest(GetQuestByGuid(guid));
        }

        public void CollectRewards(Quest quest)
        {
            if (quest.status != QuestStatus.Completed)
                return;

            var data = quest.data;

            gameProgress.skillSystem.connections
                .GetConnection(data.nation)
                .AddPoints(data.connectionPoints);

            gameProgress.fameTranslation
                .AddPoints(data.famePoints);

            foreach (var item in data.equipment)
            {
                gameProgress.inventory.Add(item);
            }

            foreach (var res in data.resources)
            {
                gameProgress.resourceContainer
                    .GetResource(res.name)
                    .GainResource(res.amount, TransactionType.QuestReward);
            }

            availableQuests.Remove(quest);
            completedQuests.Add(quest);
        }

        private void FillReplayable(Array nations)
        {
            var replCount = availableQuests
                            .Where(q => q.data.isReplayable)
                            .Count();

            if (replCount != replayableAmount)
            {
                for (int i = replCount; i < replayableAmount; i++)
                {
                    NationName nation = (NationName)nations
                        .GetValue(UnityEngine.Random.Range(1, nations.Length));

                    var data = questRegistry
                        .GetRandomReplayeableQuest(
                            nation,
                            gameProgress.skillSystem.connections.GetLevel(nation),
                            gameProgress.fameTranslation.currentFameLevel
                        );

                    if (data is not null)
                    {
                        availableQuests.Add(new Quest(data, QuestStatus.Available));
                    }
                }
            }
        }

        private void FillStoryQuests(Array nations)
        {
            foreach (NationName nation in nations)
            {
                // if we need quest with this nation
                if (availableQuests.Find(q => q.data.nation == nation) is null)
                {
                    var data = questRegistry
                        .GetRandomStoryQuest(
                            nation,
                            gameProgress.skillSystem.connections.GetLevel(nation),
                            gameProgress.fameTranslation.currentFameLevel,
                            completedQuests.Select(q => q.data.Guid)
                        );

                    if (data is not null)
                    {
                        availableQuests.Add(new Quest(data, QuestStatus.Available));
                    }
                }
            }
        }

        private Quest GetQuestByGuid(string guid)
        {
            return availableQuests.Find(q => q.data.Guid == guid);
        }
    }
}
