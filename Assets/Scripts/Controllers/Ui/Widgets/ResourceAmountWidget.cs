using Assets.Scripts.Helpers;
using Assets.Scripts.ResourceSystem;
using System;
using TMPro;
using UnityEngine;

public class ResourceAmountWidget : MonoBehaviour
{
    public TextMeshProUGUI resource;
    public TextMeshProUGUI amount;

    public void UpdateView(Resource res)
    {
        resource.SetText(Enum.GetName(typeof(ResourceName), res.name));
        amount.SetText(Converters.IntToUiString(res.amount));
    }
}
