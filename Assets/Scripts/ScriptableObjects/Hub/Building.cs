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
    public string buildingName;
    public string description;
    public bool isAvaliableAtStart = true;

    public GameObject prefab;

    public Price price;

    public abstract void InitDialogue(DialogueBox dialogueBox);
}

