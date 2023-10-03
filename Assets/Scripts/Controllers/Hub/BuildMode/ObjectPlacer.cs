using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameProgress progress;
    public List<GameObject> placedGameObjects = new();

    public int PlaceObject(Building building, BuildingPlace place)
    {
        GameObject newObject = Instantiate(building.prefab);
        newObject.transform.position = place.transform.position;
        placedGameObjects.Add(newObject);

        place.building = building;
        place.buildingIndexInPlacer = placedGameObjects.Count - 1;

        newObject.GetComponent<BuildingController>().isBuilt = true;

        progress.placedBuildings.Add(building);
        
        if (building is BonusBuilding)
            progress.globalModifiers.AddModifiers(
                building as BonusBuilding);

        return placedGameObjects.Count - 1;
    }

    public void RemoveObjectAt(BuildingPlace place)
    {
        int idx = place.buildingIndexInPlacer;

        if (placedGameObjects.Count <= idx
            || idx < 0
            || placedGameObjects[idx] == null)
        {
            return;
        }

        progress.placedBuildings.Remove(place.building);

        if (place.building is BonusBuilding)
            progress.globalModifiers.AddModifiers(
                place.building as BonusBuilding);

        Destroy(placedGameObjects[idx]);
        placedGameObjects[idx] = null;

        place.building = null;
        place.buildingIndexInPlacer = -1;
    }
}
