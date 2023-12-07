using Assets.Scripts.Helpers;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.ScriptableObjects.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Building : SerializableScriptableObject
{
    [SerializeField]
    protected ModalManager modalManager;

    public string buildingName;
    [TextArea]
    public string description;
    public bool isAvailableAtStart = true;

    public GameObject prefab;

    public Price price;

    public Building upgrade;

    public abstract void InitDialogue(DialogueBox dialogueBox,
        BuildingController controller = null);
}

