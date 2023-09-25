using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Progress/GameProgress", fileName = "GameProgress")]
public class GameProgress : ScriptableObject
{
    public int fame;
    public int maxFame = 10000;
    public int day = 1;

    public List<ItemAvaliability> buildingResearch;

    public List<BuildingPlace> buildingsPlaces;
    public List<Building> placedBuildings = new();  
    
    // other 
    [Header("Non serialized properties")]
    [Header("Registries")]
    public BuildingRegistry buildingRegistry;
    public ResourceContainer resourceContainer;

    [NonSerialized]
    public ObjectPlacer objectPlacer;

    public void AddBuilding(Vector3 placePos, string buildingGuid)
    {
        var place = buildingsPlaces.Find(p => p.transform.position == placePos);
        if (place != null) 
        {
            var building = buildingRegistry.FindByGuid(buildingGuid);

            if (building != null) 
            {
                objectPlacer.PlaceObject(building, place);
            }
        }
    }

}
