using Assets.Scripts.Controllers.Hub.BuildMode;
using Assets.Scripts.EventSystem;
using Assets.Scripts.Helpers.Enums;
using Assets.Scripts.ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    private Vector3 lastDetectedPosition = Vector3.zero;
    IBuildState buildingState;
    
    //[SerializeField]
    //private AudioClip correctPlacementClip, wrongPlacementClip;
    //[SerializeField]
    //private AudioSource source;
    
    public BuildModeInputManager inputManager;
    public BuildingRegistry buildings;
    public List<BuildingPlace> buildingPlaces;
    public PreviewSystem preview;
    public ObjectPlacer objectPlacer;
    public ResourceContainer resourceContainer;

    public GameObject confirmationDialogue;
    public Transform ui;

    public GameEvent updateUiEvent;
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
            buildings, objectPlacer, preview, 
            buildingPlaces, resourceContainer);
    }

    public void StartRemoving()
    {
        StopPlacement();
        //gridVisualization.SetActive(true);
        buildingState = new RemovingState(objectPlacer,
            preview, buildingPlaces, resourceContainer);
    }

    public void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI() || buildingState is null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();

        buildingState.OnAction(mousePosition, 
            (a, v) => StartCoroutine(ConfirmRoutine(a, v)));
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

    private IEnumerator ConfirmRoutine(Action<Vector3> action, Vector3 position)
    {
        DialogueBox dialogue = Instantiate(confirmationDialogue, ui)
            .GetComponent<DialogueBox>();

        dialogue.header = "Confirmation needed";
        dialogue.body = "Do you realy want to perform this action?";

        dialogue.modalManager.DialogueOpen(dialogue);

        while (dialogue.result == DialogueBoxResult.None)
            yield return new WaitForSeconds(0.5f);

        if (dialogue.result == DialogueBoxResult.Yes)
        {
            action.Invoke(position);
            updateUiEvent.Raise();
            StopPlacement();
        }
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
}
