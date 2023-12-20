using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGadgetHandler : MonoBehaviour
{
    public GameObject GadgetWheelCell;
    public TextMeshProUGUI TextMesh;
    public float WheelRadius = 275f;

    private GadgetHandler _gadgetHandler;

    private void Start()
    {
        _gadgetHandler = GadgetHandler.Instance;
        float angle = 360 / _gadgetHandler.GadgetIcons.Count;
        float angleRadians = Mathf.Deg2Rad * angle;
        int index = 0;
        foreach (GameObject icon in _gadgetHandler.GadgetIcons)
        {
            Vector3 position = new Vector3(WheelRadius * Mathf.Sin(angleRadians * index), WheelRadius * Mathf.Cos(angleRadians * index), 0);

            GameObject instance = Instantiate(GadgetWheelCell, transform.position, Quaternion.identity);
            instance.transform.SetParent(transform);
            instance.transform.position += position;
            instance.transform.Rotate(0, 0, -angle * index);

            GadgetWheelCellController gadgetWheelCellController = instance.GetComponent<GadgetWheelCellController>();
            gadgetWheelCellController.SelectedGadgetText = TextMesh;

            GadgetUIModel gadgetData = instance.GetComponent<GadgetUIModel>();
            gadgetData.GadgetNumber = index;
            gadgetData.GadgetName = _gadgetHandler.GadgetsData[index].Name;

            GameObject instanceIcon = Instantiate(icon, instance.transform.position, Quaternion.identity);
            instanceIcon.transform.SetParent(instance.transform);

            bool isEnabled = _gadgetHandler.UnlockedGadgets[index];

            instance.GetComponent<EventTrigger>().enabled = isEnabled;
            instance.GetComponent<GadgetWheelCellController>().enabled = isEnabled;
            instance.GetComponent<Button>().enabled = isEnabled;
            instance.GetComponent<Animator>().enabled = isEnabled;


            index++;
        }
    }
}
