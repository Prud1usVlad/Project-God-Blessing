using Assets.Scripts.Helpers;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Building : SerializableScriptableObject
{
    public string name;
    public string description;

    public GameObject prefab;

    public Price price;
}

