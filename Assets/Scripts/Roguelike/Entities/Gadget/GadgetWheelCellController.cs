using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GadgetUIModel))]
[RequireComponent(typeof(Animator))]
public class GadgetWheelCellController : MonoBehaviour
{

    public TextMeshProUGUI SelectedGadgetText;
    private GadgetUIModel _gadgetUIModel;
    private Animator _animator;
    private bool _isSelected;

    void Start()
    {
        _gadgetUIModel = GetComponent<GadgetUIModel>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isSelected)
        {
            SelectedGadgetText.text = $"{_gadgetUIModel.GadgetName}";
            SelectedGadgetText.color = GadgetHandler.Instance.UnlockedGadgets[_gadgetUIModel.GadgetNumber]
                ? Color.white
                : Color.gray;
        }
    }

    public void Selected()
    {
        _isSelected = true;
        GadgetHandler.Instance.CurrentGudgetNumber = _gadgetUIModel.GadgetNumber;
    }

    public void Deselected()
    {
        _isSelected = false;
    }

    public void HoverEnter()
    {
        _animator.SetBool(AnimatorHelper.UIAnimator.IsHoveredParameter, true);
        SelectedGadgetText.text = $"{_gadgetUIModel.GadgetName}";
        SelectedGadgetText.color = GadgetHandler.Instance.UnlockedGadgets[_gadgetUIModel.GadgetNumber]
            ? Color.white
            : Color.gray;
    }

    public void HoverExit()
    {
        _animator.SetBool(AnimatorHelper.UIAnimator.IsHoveredParameter, false);
        SelectedGadgetText.text = "";
    }
}
