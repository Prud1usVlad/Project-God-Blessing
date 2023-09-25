using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsumptionWidget : MonoBehaviour
{
    public TextMeshProUGUI resourceName;
    public TextMeshProUGUI pointConsumption;
    public TextMeshProUGUI allConsumption;

    public void UpdateView(string name, string pointC, string allC)
    {
        resourceName.SetText(name);
        pointConsumption.SetText(pointC);
        allConsumption.SetText(allC);
    }
}
