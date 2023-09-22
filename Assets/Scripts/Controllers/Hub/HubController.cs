using Assets.Scripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour
{
    public List<BuildingPlace> buildingPlaces;
    public BuildingRegistry buildings;

    public GameObject buildSystem;

    public void OnEnterBuildMode()
    {
        buildSystem.SetActive(true);
    }

    public void OnExitBuildMode() 
    {
        buildSystem.SetActive(false);
    }

}
