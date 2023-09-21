using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlace : MonoBehaviour
{
    public Building building;


    public void OnEnterBuildingMode()
    {
        if (building is null) 
        {
            // ...
        }
    }
}
