using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Progress/GameProgress", fileName = "GameProgress")]
public class GameProgress : ScriptableObject
{
    public int day = 1;

    public List<ItemAvaliability> buildingResearch;

    public List<Building> placedBuildings = new();  
    public List<CurseCard> curses = new();

    // other 
    [Header("Non serialized properties")]
    [Header("Registries")]
    public BuildingRegistry buildingRegistry;
    public CurseRegistry curseRegistry;
    public ResourceContainer resourceContainer;
    [Header("Translations")]
    public FameTranslation fameTranslation;
    public LiesTranslation liesTranslation;


    private void OnEnable()
    {
        placedBuildings = new();
        curses = new();
    }
}
