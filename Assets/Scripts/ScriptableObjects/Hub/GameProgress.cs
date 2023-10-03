using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.StatSystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Progress/GameProgress", fileName = "GameProgress")]
public class GameProgress : ScriptableObject
{
    public int day = 1;

    [Header("Progress")]
    public List<ItemAvaliability> buildingResearch;

    [Header("Runtime game data")]
    public List<Building> placedBuildings = new();  
    public List<CurseCard> curses = new();

    // other 
    [Header("Registries")]
    public BuildingRegistry buildingRegistry;
    public CurseRegistry curseRegistry;
    public ResourceContainer resourceContainer;
    [Header("Translations")]
    public FameTranslation fameTranslation;
    public LiesTranslation liesTranslation;
    [Header("Data trackers")]
    public GlobalModifiers globalModifiers;

    private void OnEnable()
    {
        placedBuildings = new();
        curses = new();
    }
}
