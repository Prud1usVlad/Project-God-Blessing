using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/ConnectionsContainer", fileName = "ConnectionsContainer")]
public class ConnectionsContainer : ScriptableObject, IEnumerable<ConnectionsTranslation>
{
    [SerializeField]
    private List<ConnectionsTranslation> connections;

    public ConnectionsTranslation GetConnection(NationName nation)
    {
        return connections?.Find(c => c.nation == nation);
    }

    public int GetLevel(NationName nation)
    {
        var conn = GetConnection(nation);
        return conn is null ? -1 : conn.currentLevelIdx;
    }

    public void AddPoints(int amount, NationName nation)
    {
        GetConnection(nation).AddPoints(amount);
    }

    public IEnumerator<ConnectionsTranslation> GetEnumerator()
    {
        return connections.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return connections.GetEnumerator();
    }
}
