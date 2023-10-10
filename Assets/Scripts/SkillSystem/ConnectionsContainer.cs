using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/ConnectionsContainer", fileName = "ConnectionsContainer")]
public class ConnectionsContainer : ScriptableObject
{
    [SerializeField]
    private List<ConnectionsTranslation> connections;

    public ConnectionsTranslation GetConnection(NationName nation)
    {
        return connections?.Find(c => c.nation == nation);
    }

    public void AddPoints(int amount, NationName nation)
    {
        GetConnection(nation).AddPoints(amount);
    }
}
