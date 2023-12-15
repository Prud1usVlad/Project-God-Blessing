using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductionWidget : MonoBehaviour
{
    public TextMeshProUGUI resourceName;
    public TextMeshProUGUI resourceProduction;

    public void UpdateView(string name, string prod) 
    {
        resourceName.SetText(name);
        resourceProduction.SetText(prod);
    }

}
