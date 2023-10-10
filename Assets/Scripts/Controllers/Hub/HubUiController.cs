using Assets.Scripts.EventSystem;
using Assets.Scripts.ResourceSystem;
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
    }

    public void OnExitBuildingMode()
    {
        basicUi.gameObject.SetActive(true);
        buildingModeUi.gameObject.SetActive(false);

        exitBuildMode.Raise();
    }

    public void OnRemoveBuildings()
    {
        removeMode.Raise();
    }

    public void OnUpdateUi()
    {
        buildingsListView.RefreshList();
    }
}
