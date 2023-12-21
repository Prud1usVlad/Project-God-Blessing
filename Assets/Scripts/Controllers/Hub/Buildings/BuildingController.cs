using Assets.Scripts.ScriptableObjects.Hub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuildingController : MonoBehaviour
{
    public bool isInBuildMode = false;

    public ModalManager modalManager;
    public GameProgress gameProgress;
    public HubController hubController;

    public Building building;
    public GameObject dialogueBox;
    public bool isBuilt = false;

    private void Awake()
    {
        hubController = FindAnyObjectByType<HubController>();
    }

    public void Interact()
    {
        var canvas = GameObject.FindGameObjectWithTag("MainUi");
        var dial = Instantiate(dialogueBox, canvas.transform);
        var comp = dial.GetComponent<DialogueBox>();

        building.InitDialogue(comp, this);
    }

    public void OnBuildModeEnter()
    {
        isInBuildMode = true;
    }

    public void OnBuildModeExit()
    {
        isInBuildMode = false;
    }

    public bool HasUpgrades() =>
        building.upgrade is not null;

    public bool CanUpgrade()
    {
        if (HasUpgrades())
        {
            var reserched = gameProgress.buildingResearch?
                .Find(b => b.guid == building.upgrade.Guid)?.isAvailable;

            if ((reserched ?? false) && gameProgress.resourceContainer.CanAfford(building.upgrade.price))
            {
                return true;

            } 
        }


        return false;
    }

    public void UpdgradeBuilding()
    {
        if (CanUpgrade())
        {
            gameProgress.resourceContainer.Spend(building.upgrade.price);
            building = building.upgrade;

            hubController.UpgradeBuilding(gameObject.transform.position);
        }
    }


    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !isInBuildMode)
            Interact();
    }
}
