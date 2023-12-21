using System;
using Assets.Scripts.ResourceSystem;
using UnityEngine;

[Serializable]
public class AvailableResource
{
    public ResourceName Name;
    public int MinAmount;
    public int MaxAmount;

    [Range(0.01f, 1f)]
    public float LootChanse;
}
