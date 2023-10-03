using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlace : MonoBehaviour
{
    public Building building = null;
    public int buildingIndexInPlacer = -1;

    public void OnEnterBuildingMode()
    {
        if (building is null) 
        {
            // ...
        }
    }
}
