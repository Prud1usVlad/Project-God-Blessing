using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public List<GameObject> placedGameObjects = new();

    public int PlaceObject(Building building, BuildingPlace place)
    {
        GameObject newObject = Instantiate(building.prefab);
        newObject.transform.position = place.transform.position;
        placedGameObjects.Add(newObject);

        place.building = building;
        place.buildingIndexInPlacer = placedGameObjects.Count - 1;

        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(BuildingPlace place)
    {
        int idx = place.buildingIndexInPlacer;

        if (placedGameObjects.Count <= idx
            || idx < 0
            || placedGameObjects[idx] == null)
        {
            return;
        }
            
        Destroy(placedGameObjects[idx]);
        placedGameObjects[idx] = null;

        place.building = null;
        place.buildingIndexInPlacer = -1;
    }
}
