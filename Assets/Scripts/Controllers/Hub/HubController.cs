using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.SaveSystem.SaveFile;

public class HubController : MonoBehaviour
{
    private ObjectPlacer objectPlacer;

    public List<BuildingPlace> buildingPlaces;
    public BuildingRegistry buildings;

    public GameObject buildSystem;

    public SaveController saveController;

    public GameProgress gameProgress;

    public void OnEnterBuildMode()
    {
        buildSystem.SetActive(true);
    }

    public void OnExitBuildMode() 
    {
        buildSystem.SetActive(false);
    }

    private void Awake()
    {
        objectPlacer = buildSystem.GetComponent<ObjectPlacer>();
        saveController.Load();

        gameProgress.questSystem.FillAvailable();
        var market = gameProgress.placedBuildings.Find(b => b is MarketBuilding);

        if (market != null)
            (market as MarketBuilding).UpdateStore(); 
    }

    private void OnDestroy()
    {
        Debug.Log("Scene unload");
        saveController.AutoSave();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            //SceneManager.LoadScene(1);
        }
    }

    public void AddBuildings(List<Place> places)
    {
        foreach (var savedPlace in places)
        {
            var place = buildingPlaces.Find(p => p.transform.position == savedPlace.position);
            if (place != null)
            {
                var building = buildings.FindByGuid(savedPlace.buildingGuid);

                if (building != null)
                {
                    objectPlacer.PlaceObject(building, place);
                }
            }
        }
    }
}
