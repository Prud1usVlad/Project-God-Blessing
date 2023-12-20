using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GadgetHandler : MonoBehaviour
{
    public List<GameObject> Gadgets;
    public List<GameObject> GadgetIcons;
    public List<GadgetData> GadgetsData;
    public List<bool> UnlockedGadgets;

    public int CurrentGudgetNumber = 0;

    public static GadgetHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GadgetHandler>();

                if (_instance == null)
                {
                    throw new Exception("Gadget handler does not exist");
                }
            }

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    private static GadgetHandler _instance;

    private GadgetHandler() { }

    private void Start()
    {
        if (Gadgets.Count != GadgetIcons.Count)
        {
            throw new Exception("Gadgets amount and they icons amount does not equal");
        }

        if (UnlockedGadgets.Count > Gadgets.Count)
        {
            throw new Exception("Unlocked gadgets amount bigger than existing ones");
        }

        CurrentGudgetNumber = UnlockedGadgets.IndexOf(UnlockedGadgets.FirstOrDefault(x => x));
    }

    private void Update()
    {
        if (CurrentGudgetNumber == -1)
        {
            return;
        }

        if (!UnlockedGadgets[CurrentGudgetNumber])
        {
            throw new Exception("Locked gadget is choosed");
        }
    }
}
