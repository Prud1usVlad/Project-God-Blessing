using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HubController : MonoBehaviour
{
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
        gameProgress.buildingsPlaces = buildingPlaces;
        gameProgress.objectPlacer = buildSystem.GetComponent<ObjectPlacer>();

        saveController.Load();
    }

    private void OnDestroy()
    {
        Debug.Log("Scene unload");
        saveController.AutoSave();
    }
}
