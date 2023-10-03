using Assets.Scripts.Helpers;
using Assets.Scripts.Registries;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Registries/Curse", fileName = "CurseRegistry")]
public class CurseRegistry : Registry<CurseCard>, IListViewExtendedRegistry
{
    public CurseCard InitByGuid(string guid, int propIdx, int imgIdx)
    {
        var c = FindByGuid(guid);
        c.Init(propIdx, imgIdx);
        return c;
    }

    public CurseCard GetRandom(int level)
    {
        var cards = _descriptors.Where(c => c.level == level);
        var card = cards.ElementAt(Random.Range(0, cards.Count()));
        card.Randomize();
        return card;
    }

    public void ForEach(System.Action<SerializableScriptableObject> action)
    {
        foreach (var card in _descriptors)
        {
            action(card);
        }
    }

    public SerializableScriptableObject Find(string guid)
    {
        return FindByGuid(guid);
    }
}
