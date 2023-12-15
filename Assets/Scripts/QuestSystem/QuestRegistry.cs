using Assets.Scripts.QuestSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/Quest")]
public class QuestRegistry : Registry<QuestData>
{
    public QuestData GetRandomStoryQuest(NationName nation, 
        int connectionLevel, FameLevel fameLevel, IEnumerable<string> exclude = null)
    {
        return GetRandomBase(nation, connectionLevel, fameLevel, false, exclude);
    }

    public QuestData GetRandomReplayeableQuest(NationName nation, 
        int connectionLevel, FameLevel fameLevel, IEnumerable<string> exclude = null)
    {
        return GetRandomBase(nation, connectionLevel, fameLevel, true, exclude);
    }

    private QuestData GetRandomBase(NationName nation,
        int connectionLevel, FameLevel fameLevel, 
        bool isRep, IEnumerable<string> exclude = null)
    {
        var dataset = _descriptors
            .Where(q => q.isReplayable == isRep)
            .Where(q => q.nation == nation
                && q.connectionLevel == connectionLevel
                && q.fameLevel == fameLevel);

        if (exclude is not null)
            dataset = dataset.Where(q => !exclude.Contains(q.Guid));

        if (dataset.Count() == 0)
            return null;


        var rand = Random.Range(0, dataset.Count());

        return dataset.ElementAt(rand);
    }
}