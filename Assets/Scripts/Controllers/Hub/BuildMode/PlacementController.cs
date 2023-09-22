using Assets.Scripts.Controllers.Hub.BuildMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    private ControllerState state = ControllerState.None;

    [SerializeField]
    private BuildModeInputManager inputManager;

    //[SerializeField]
    //private AudioClip correctPlacementClip, wrongPlacementClip;
    //[SerializeField]
    //private AudioSource source;

    [SerializeField]
    private BuildingRegistry buildings;
    [SerializeField]
    private List<BuildingPlace> buildingPlaces;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3 lastDetectedPosition = Vector3.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildState buildingState;

    //[SerializeField]
    //private SoundFeedback soundFeedback;

    private void Start()
    {
        //gridVisualization.SetActive(false);
    }
    private void OnDisable()
    {
        StopPlacement();
    }

    public void StartPlacement(string guid)
    {
        StopPlacement();
        //gridVisualization.SetActive(true);
        buildingState = new PlacementState(guid, 
            buildings, objectPlacer, preview, buildingPlaces);
    }

    public void StartRemoving()
    {
        StopPlacement();
        //gridVisualization.SetActive(true);
        //buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer, soundFeedback);
        //inputManager.OnClicked += PlaceStructure;
        //inputManager.OnExit += StopPlacement;
    }

    public void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        Debug.Log("PlaceStructure");
        buildingState.OnAction(mousePosition);

    }

    public void StopPlacement()
    {
        //soundFeedback.PlaySound(SoundType.Click);
        if (buildingState == null)
            return;
        //gridVisualization.SetActive(false);
        buildingState.EndState();

        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        if (lastDetectedPosition != mousePosition)
        {
            buildingState.UpdateState(mousePosition);
            lastDetectedPosition = mousePosition;
        }
    }

    private enum ControllerState
    {
        None,
        Placing,
        Removing,
    }
}
