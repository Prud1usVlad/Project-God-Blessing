using Assets.Scripts.EventSystem;
using Assets.Scripts.ResourceSystem;
using Assets.Scripts.ScriptableObjects.Hub;
using TMPro;
using UnityEngine;

public class HubUiController : MonoBehaviour
{
    [Header("Ui elements")]
    public ListViewController buildingsListView;

    public Transform buildingModeUi;
    public Transform basicUi;

    [Header("Data")]
    public BuildingRegistry buildings;
    public ResourceContainer resources;
    public RuntimeHubUiData runtimeData;

    [Header("Events")]
    public GameEvent enterBuildMode;
    public GameEvent exitBuildMode;
    public GameEvent removeMode;
    public GameEvent addDay;

    public void OnEnterBuildMode()
    {
        basicUi.gameObject.SetActive(false);
        buildingModeUi.gameObject.SetActive(true);

        var list = buildingModeUi.GetComponentInChildren<ListViewController>();
        list.InitView(buildings);

        enterBuildMode.Raise();
        runtimeData.isInBuildMode = true;
    }

    public void OnExitBuildingMode()
    {
        basicUi.gameObject.SetActive(true);
        buildingModeUi.gameObject.SetActive(false);

        exitBuildMode.Raise();
        runtimeData.isInBuildMode = false;
    }

    public void OnRemoveBuildings()
    {
        removeMode.Raise();
    }

    public void OnUpdateUi()
    {
        buildingsListView.RefreshList();
    }

    public void Update()
    {
        if (runtimeData.isDialogOpened && basicUi.gameObject.activeSelf)
            basicUi.gameObject.SetActive(false);
        else if (!runtimeData.isDialogOpened
            && !runtimeData.isInBuildMode
            && !basicUi.gameObject.activeSelf)
            basicUi.gameObject.SetActive(true);
    }
}
