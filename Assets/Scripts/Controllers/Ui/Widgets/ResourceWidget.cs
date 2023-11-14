using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResourceWidget : MonoBehaviour
{
    public TextMeshProUGUI _name;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI gained;
    public TextMeshProUGUI used;

    public void UpdateView(string name, 
        int amount, int used, int gained)
    {
        _name.SetText(name);
        this.amount.SetText(Converters.IntToUiString(amount));
        this.gained.SetText(Converters.IntToUiString(gained));
        this.used.SetText(Converters.IntToUiString(used));
    }
}
