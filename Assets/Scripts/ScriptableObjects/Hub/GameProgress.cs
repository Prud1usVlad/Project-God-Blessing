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
    public Equipment equipment;
    public Inventory inventory;

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
    public SkillSystem skillSystem;

    private void OnEnable()
    {
        placedBuildings = new();
        curses = new();
    }

    public void AddBuilding(Building building)
    {
        placedBuildings.Add(building);

        if (building is BonusBuilding)
        {
            globalModifiers.AddModifiers(
                building as BonusBuilding);
        }
        else if (building is ProductionBuilding)
        {
            resourceContainer.productionBuildings
                .Add(building as ProductionBuilding);
        }

    }

    public void RemoveBuilding(Building building) 
    {
        placedBuildings.Remove(building);

        if (building is BonusBuilding)
        {
            globalModifiers.AddModifiers(
                building as BonusBuilding);
        }
        else if (building is ProductionBuilding)
        {
            resourceContainer.productionBuildings
                .Add(building as ProductionBuilding);
        }
    }

    public void AddCurse(CurseCard curse)
    {
        curses.Add(curse);
        globalModifiers.AddModifiers(curse);
    }

    public void RemoveCurse(CurseCard curse)
    {
        curses.Remove(curse);
        globalModifiers.RemoveModifiers(curse);
    }
}
