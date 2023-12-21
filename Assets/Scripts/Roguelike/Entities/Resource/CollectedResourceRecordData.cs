using Assets.Scripts.ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectedResourceRecordData : MonoBehaviour
{
    public TextMeshProUGUI ResourceLabel;
    public Image ResourceIcon;
    public ResourceName ResourceName;
    private int _collectedAmount;
    public int CollectedAmount
    {
        get
        {
            return _collectedAmount;
        }
        set
        {
            _collectedAmount = value;
            ResourceLabel.text = $"X {_collectedAmount}";
        }
    }

}
