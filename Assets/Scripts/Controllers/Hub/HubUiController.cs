using Assets.Scripts.EventSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HubUiController : MonoBehaviour
{
    [Header("Ui elements")]
    public Transform buildingModeUi;
    public Transform basicUi;

    [Header("Data")]
    public BuildingRegistry buildings;

    [Header("Events")]
    public GameEvent enterBuildMode;
    public GameEvent exitBuildMode;

    public void OnEnterBuildMode()
    {
        basicUi.gameObject.SetActive(false);
        buildingModeUi.gameObject.SetActive(true);

        var list = buildingModeUi.GetComponentInChildren<ListViewController>();
        list.InitView(buildings);

        enterBuildMode.Raise();
    }

    public void OnExitBuildingMode()
    {
        basicUi.gameObject.SetActive(true);
        buildingModeUi.gameObject.SetActive(false);

        exitBuildMode.Raise();
    }
}
