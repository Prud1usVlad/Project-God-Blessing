using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlace : MonoBehaviour
{
    [SerializeField]
    private Building building = null;
    [SerializeField]
    private int buildingIndexInPlacer = -1;

    public GameObject decorations;


    public Building Building { get => building; }
    public int BuildingIndexInPlacer { get => buildingIndexInPlacer; }

    public void OnEnterBuildingMode()
    {
        if (Building is null) 
        {
            // ...
        }
    }

    public void SetBuilding(Building building, int index)
    {
        this.building = building;
        this.buildingIndexInPlacer = index;

        SideEffect();
    }

    public void RemoveBuilding()
    {
        building = null;
        buildingIndexInPlacer = -1;

        SideEffect();
    }

    protected void SideEffect()
    {
        if (building is null)
        {
            decorations.SetActive(true);
        }
        else
        {
            decorations.SetActive(false);
        }
    } 
}
