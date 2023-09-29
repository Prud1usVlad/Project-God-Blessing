using Assets.Scripts.Models;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Progress/GameProgress", fileName = "GameProgress")]
public class GameProgress : ScriptableObject
{
    public int day = 1;

    public List<ItemAvaliability> buildingResearch;

    // init from hub contoller
    //public List<BuildingPlace> buildingsPlaces;
    public List<Building> placedBuildings = new();  
    
    // other 
    [Header("Non serialized properties")]
    [Header("Registries")]
    public BuildingRegistry buildingRegistry;
    public ResourceContainer resourceContainer;
    [Header("Translations")]
    public FameTranslation fameTranslation;

}
